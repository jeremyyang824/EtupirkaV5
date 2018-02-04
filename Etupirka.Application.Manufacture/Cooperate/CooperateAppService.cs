using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Abp.Linq.Extensions;
using Castle.Core.Logging;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.External.Bapi;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.External.Entities.Fs;
using Etupirka.Domain.External.Entities.Fsti;
using Etupirka.Domain.External.Entities.Vmes;
using Etupirka.Domain.External.Fsti;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Manufacture.Services;
using Etupirka.Domain.Portal;

namespace Etupirka.Application.Manufacture.Cooperate
{
    /// <summary>
    /// 工艺外协管理
    /// </summary>
    [AbpAuthorize]
    public class CooperateAppService : EtupirkaAppServiceBase, ICooperateAppService
    {
        private readonly IRepository<SapMOrder, Guid> _sapMOrderRepository;
        private readonly IRepository<SapMOrderProcess, Guid> _sapMOrderProcessRepository;
        private readonly IRepository<SapMOrderProcessCooperate, int> _sapMOrderProcessCooperateRepository;
        private readonly IRepository<SapMOrderProcessCooperateStep, int> _sapMOrderProcessCooperateStepRepository;
        private readonly IRepository<ProcessCodeMap, int> _processCodeMap;
        private readonly SapMOrderManager _sapMOrderManager;
        private readonly IFSTIRepository _fstiRepository;
        private readonly IFSRepository _fsRepository;
        private readonly IFSTIHelper _fstiHelper;
        private readonly IBAPIRepository _bapiRepository;
        private readonly IVMESRepository _vmesRepository;
        private readonly SapSupplierManager _sapSupplierManager;
        private readonly CooperateConfigurations _cooperateConfigurations;

        public ILogger Logger { get; set; }

        public CooperateAppService(
            IRepository<SapMOrder, Guid> sapMOrderRepository,
            IRepository<SapMOrderProcess, Guid> sapMOrderProcessRepository,
            IRepository<SapMOrderProcessCooperate, int> sapMOrderProcessCooperateRepository,
            IRepository<SapMOrderProcessCooperateStep, int> sapMOrderProcessCooperateStepRepository,
            IRepository<ProcessCodeMap, int> processCodeMap,
            SapMOrderManager sapMOrderManager,
            IFSTIRepository fstiRepository,
            IFSRepository fsRepository,
            IFSTIHelper fstiHelper,
            IBAPIRepository bapiRepository,
            IVMESRepository vmesRepository,
            SapSupplierManager sapSupplierManager,
            CooperateConfigurations cooperateConfigurations)
        {
            this._sapMOrderRepository = sapMOrderRepository;
            this._sapMOrderProcessRepository = sapMOrderProcessRepository;
            this._sapMOrderProcessCooperateRepository = sapMOrderProcessCooperateRepository;
            this._sapMOrderProcessCooperateStepRepository = sapMOrderProcessCooperateStepRepository;
            this._processCodeMap = processCodeMap;
            this._sapMOrderManager = sapMOrderManager;
            this._fstiRepository = fstiRepository;
            this._fsRepository = fsRepository;
            this._fstiHelper = fstiHelper;
            this._bapiRepository = bapiRepository;
            this._vmesRepository = vmesRepository;
            this._sapSupplierManager = sapSupplierManager;
            this._cooperateConfigurations = cooperateConfigurations;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 同步SAP订单到本地
        /// </summary>
        public async Task SapMOrderSync(SapMOrderSyncInput input)
        {
            //try
            //{
            //获取SAP订单
            var sapOrderOutput = _bapiRepository.GetSapOrders(new GetSapOrdersInput
            {
                Plant = _cooperateConfigurations.Cooperate_SapWestWERKS,    //西厂区
                OrderNumberRangeBegin = input.OrderNumberRangeBegin,
                OrderNumberRangeEnd = input.OrderNumberRangeEnd,
            });

            foreach (var sapOrder in sapOrderOutput.OrderList)
            {
                //查询现有订单
                var existOrder = await _sapMOrderRepository
                    .GetAll()
                    .Include(o => o.OrderProcess)
                    .FirstOrDefaultAsync(o => o.OrderNumber == sapOrder.OrderNumber);

                //不存在，新增
                if (existOrder == null)
                {
                    await this.createSapMOrder(sapOrder);
                }
                //存在，更新
                else
                {
                    await this.updateSapMOrder(sapOrder, existOrder);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(ex.Message, ex);
            //}
        }

        //创建订单（及工艺、外协）信息
        private async Task createSapMOrder(BapiOrderOutput sapOrder)
        {
            //创建订单
            SapMOrder orderEntity = sapOrder.MapTo<SapMOrder>();
            orderEntity.Id = this.GuidGenerator.Create();
            orderEntity.OrderProcess = new List<SapMOrderProcess>();
            foreach (var sapProcess in sapOrder.BapiOrderProcessList)
            {
                //创建工序
                SapMOrderProcess processEntity = sapProcess.MapTo<SapMOrderProcess>();
                processEntity.Id = this.GuidGenerator.Create();
                processEntity.SapMOrderId = orderEntity.Id;
                processEntity.SapMOrder = orderEntity;
                orderEntity.OrderProcess.Add(processEntity);
            }
            await this._sapMOrderRepository.InsertAndGetIdAsync(orderEntity);

            //创建工艺外协
            foreach (var processEntity in orderEntity.OrderProcess)
            {
                if (processEntity.CanCooperate())
                {
                    string sapSupplierCode = processEntity.LIFNR;
                    var sapSupplierMapper = string.IsNullOrWhiteSpace(sapSupplierCode) ?
                        SapSupplierMaper.Empty :
                        await _sapSupplierManager.GetSupplierBySapCode(sapSupplierCode);    //取得供应商信息

                    var cooperateEntity = new SapMOrderProcessCooperate
                    {
                        SapMOrderProcessId = processEntity.Id,
                        SapMOrderProcess = processEntity,
                        CooperateType = sapSupplierMapper.IsFsSupplier ? SapMOrderProcessCooperateType.ToForthShift : SapMOrderProcessCooperateType.ToOutsideSupplier,
                        CooperaterCode = sapSupplierCode,
                        CooperaterName = sapSupplierMapper.SupplierName,
                        CooperaterFsPointOfUse = sapSupplierMapper.FsPointOfUse,
                        CooperaterPrice = 0M
                    };
                    await this._sapMOrderProcessCooperateRepository.InsertAsync(cooperateEntity);
                }
            }
        }

        private async Task updateSapMOrder(BapiOrderOutput bapiOrder, SapMOrder existOrder)
        {
            //更新订单
            bapiOrder.MapTo(existOrder);
            Dictionary<string, SapMOrderProcess> existProcessList
                = existOrder.OrderProcess.ToDictionary(p => p.OperationNumber);   // 工序号为索引
            foreach (var bapiProcess in bapiOrder.BapiOrderProcessList.OrderBy(p => p.OperationNumber))
            {
                SapMOrderProcess existProcess = null;
                bool isUpdateProcess = existProcessList.TryGetValue(bapiProcess.OperationNumber, out existProcess);
                if (isUpdateProcess)
                {
                    //更新道序
                    bapiProcess.MapTo(existProcess);
                    existProcessList.Remove(existProcess.OperationNumber);
                }
                else
                {
                    //新增道序
                    SapMOrderProcess newProcess = bapiProcess.MapTo<SapMOrderProcess>();
                    newProcess.Id = this.GuidGenerator.Create();
                    newProcess.SapMOrderId = existOrder.Id;
                    newProcess.SapMOrder = existOrder;
                    existOrder.OrderProcess.Add(newProcess);
                }
            }
            //删除道序
            foreach (var deleteProcess in existProcessList.Values)
            {
                deleteProcess.SapMOrder = null;
                existOrder.OrderProcess.Remove(deleteProcess);
                await this._sapMOrderProcessRepository.DeleteAsync(deleteProcess);

                //删除道序外协信息
                var existsCooperate = await this._sapMOrderProcessCooperateRepository.FirstOrDefaultAsync(
                    co => co.SapMOrderProcessId == deleteProcess.Id);
                await this._sapMOrderProcessCooperateRepository.DeleteAsync(existsCooperate);
            }
            await this._sapMOrderRepository.UpdateAsync(existOrder);

            //调整工艺外协
            foreach (var processEntity in existOrder.OrderProcess)
            {
                var existsCooperate = await this._sapMOrderProcessCooperateRepository.FirstOrDefaultAsync(
                    co => co.SapMOrderProcessId == processEntity.Id);

                if (processEntity.CanCooperate() && existsCooperate == null)
                {
                    string sapSupplierCode = processEntity.LIFNR;
                    var sapSupplierMapper = string.IsNullOrWhiteSpace(sapSupplierCode) ?
                        SapSupplierMaper.Empty :
                        await _sapSupplierManager.GetSupplierBySapCode(sapSupplierCode);    //取得供应商信息

                    //创建工艺外协
                    var cooperateEntity = new SapMOrderProcessCooperate
                    {
                        SapMOrderProcessId = processEntity.Id,
                        SapMOrderProcess = processEntity,
                        CooperateType = sapSupplierMapper.IsFsSupplier ? SapMOrderProcessCooperateType.ToForthShift : SapMOrderProcessCooperateType.ToOutsideSupplier,
                        CooperaterCode = sapSupplierCode,
                        CooperaterName = sapSupplierMapper.SupplierName,
                        CooperaterFsPointOfUse = sapSupplierMapper.FsPointOfUse,
                        CooperaterPrice = 0M
                    };
                    await this._sapMOrderProcessCooperateRepository.InsertAsync(cooperateEntity);
                }
                else if (processEntity.CanCooperate() && existsCooperate != null)
                {
                    string sapSupplierCode = processEntity.LIFNR;
                    var sapSupplierMapper = string.IsNullOrWhiteSpace(sapSupplierCode) ?
                        SapSupplierMaper.Empty :
                        await _sapSupplierManager.GetSupplierBySapCode(sapSupplierCode);    //取得供应商信息

                    //更新工艺外协
                    existsCooperate.CooperateType = sapSupplierMapper.IsFsSupplier ? SapMOrderProcessCooperateType.ToForthShift : SapMOrderProcessCooperateType.ToOutsideSupplier;
                    existsCooperate.CooperaterCode = sapSupplierCode;
                    existsCooperate.CooperaterName = sapSupplierMapper.SupplierName;
                    existsCooperate.CooperaterFsPointOfUse = sapSupplierMapper.FsPointOfUse;
                    existsCooperate.CooperaterPrice = 0M;

                    await this._sapMOrderProcessCooperateRepository.UpdateAsync(existsCooperate);
                }
            }
        }

        /// <summary>
        /// SAP订单工艺外协送出
        /// </summary>
        [RemoteService(IsEnabled = false)]
        [UnitOfWork(isTransactional: false)]
        public async Task<bool> SapCooperSendOut(SapCooperSendInput input)
        {
            //找到工序信息
            var sapProcess = await this._sapMOrderProcessRepository.GetAll()
                .Include(p => p.SapMOrder)
                .Where(p => p.SapMOrder.OrderNumber == input.SapMOrderNumber && p.OperationNumber == input.SapMOrderProcessNumber)
                .FirstOrDefaultAsync();
            if (sapProcess == null)
                throw new DomainException($"工序{input.SapMOrderNumber}/{input.SapMOrderProcessNumber}不存在！");
            if (!sapProcess.CanCreateErpInterface())
                return true;    //不需要创建任何ERP接口

            //外协信息
            var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                .FirstOrDefaultAsync(co => co.SapMOrderProcessId == sapProcess.Id);
            if (sapCooperate == null || string.IsNullOrWhiteSpace(sapCooperate.CooperaterCode))
                throw new DomainException($"工序{input.SapMOrderNumber}/{input.SapMOrderProcessNumber}外协信息不存在！");
            //if (sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToOutsideSupplier && sapCooperate.CooperaterPrice <= 0)
            //    throw new DomainException($"工序{input.SapMOrderNumber}/{input.SapMOrderProcessNumber}外协价格未写入！");

            try
            {
                bool result = true;

                //前序道序批量入库
                result = await sapPreviousProcessReceive(sapProcess, sapCooperate);
                if (result)
                {
                    result = await this.coreFlow(sapCooperate, sapProcess, input.HandOverQuantity);
                }
                return result;
            }
            catch (Exception ex)
            {
                //其他异常记录
                await this._sapMOrderProcessCooperateStepRepository.InsertAsync(new SapMOrderProcessCooperateStep
                {
                    CooperateInfo = sapCooperate,
                    StepTransactionType = SapMOrderProcessCooperateStepTransTypes.Others,
                    StepName = SapMOrderProcessCooperateStepTransTypes.Others,
                    IsStepSuccess = false,
                    StepResultMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 1800))
                });
                return false;
            }
        }

        private async Task<bool> coreFlow(SapMOrderProcessCooperate sapCooperate, SapMOrderProcess sapProcess, decimal handOverQuantity)
        {
            if (!sapProcess.CanCreateErpInterface())
                return true;

            bool result = true;
            if (sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift)
            {
                //创建一个道序的SAP采购订单
                //准备工作
                result = await this.stepPrepare(sapProcess, sapCooperate, handOverQuantity);

                //SAP采购申请审批
                if (result)
                    result = await this.stepSapPoRequestRelease(sapProcess, sapCooperate);

                //创建采购订单
                if (result)
                    result = await this.stepSapPomt(sapProcess, sapCooperate);

                using (var context = _fstiHelper.BeginFsti()) //登入FSTI
                {
                    //创建FS销售订单
                    if (result)
                        result = await this.stepFsComt(context.FstiToken, sapProcess, sapCooperate);

                    //创建FS生产订单
                    if (result)
                        result = await this.stepFsMomt(context.FstiToken, sapProcess, sapCooperate);

                    //创建FS发料清单（PICK）
                    if (result)
                        result = await this.stepFsPick(context.FstiToken, sapProcess, sapCooperate);
                }
            }
            else
            {
                //根据连续的相同外协供应商道序，创建采购订单
                var operList = await this.getContinuousSupplierProcess(sapProcess.SapMOrder, sapProcess.OperationNumber, true);

                foreach (var oper in operList)
                {
                    //准备工作
                    if (result)
                        result = await this.stepPrepare(oper.SapMOrderProcess, oper, handOverQuantity);

                    //SAP采购申请审批
                    if (result)
                        result = await this.stepSapPoRequestRelease(oper.SapMOrderProcess, oper);
                }

                //创建一个多行的采购订单
                result = await this.stepSapPomtBatch(operList);
            }
            return result;
        }

        /// <summary>
        /// SAP工艺外协创建的FS制造订单道序完工
        /// </summary>
        [RemoteService(IsEnabled = false)]
        [UnitOfWork(isTransactional: false)]
        [AbpAllowAnonymous]
        public async Task<bool> SapCooperFsProcessFinished(SapCooperInspectedInput input)
        {
            //找到Sap外协道序
            var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                .Include(co => co.SapMOrderProcess)
                .OrderByDescending(co => co.Id)
                .FirstOrDefaultAsync(co => co.FsMoNumber == input.FsMOrderNumber && co.CooperateType == SapMOrderProcessCooperateType.ToForthShift);
            if (sapCooperate == null)
            {
                return true;    //非SAP-FS订单
                //throw new DomainException($"FS订单{input.FsMOrderNumber}找不到对应的SAP外协信息！");
            }

            var currentSapProcess = sapCooperate.SapMOrderProcess;
            bool result = sapCooperate.IsFsPickFinished != null && sapCooperate.IsFsPickFinished.Value;   //FS发料完成PICK

            try
            {
                //反写质检完成标识
                if (result)
                {
                    sapCooperate.IsMesInspectFinished = true;
                    sapCooperate.MesInspectQualified = input.InspectQualified;
                    await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
                }

                using (var context = _fstiHelper.BeginFsti()) //登入FSTI
                {
                    //送检MORV
                    if (result)
                        result = await this.stepFsMorv(context.FstiToken, currentSapProcess, sapCooperate);

                    //移合格库IMTR
                    if (result)
                        result = await this.stepFsImtr(context.FstiToken, currentSapProcess, sapCooperate);

                    //移发运库IMTR
                    if (result)
                        result = await this.stepFsImtrSales(context.FstiToken, currentSapProcess, sapCooperate);

                    //发运SHIP
                    if (result)
                        result = await this.stepFsShip(context.FstiToken, currentSapProcess, sapCooperate);

                    //SAP采购订单审批
                    if (result)
                        result = await this.stepSapPoRelease(currentSapProcess, sapCooperate);

                    //SAP采购订单入库
                    if (result)
                        result = await this.stepSapPorv(currentSapProcess, sapCooperate);

                    //检查下个道序是否继续外协
                    if (result)
                    {
                        var nextSapCooperate = (await this._sapMOrderManager.GetNextSapMOrderProcess(currentSapProcess.Id))?.CooperateLine;
                        if (nextSapCooperate != null)
                        {
                            result = await this.coreFlow(nextSapCooperate, nextSapCooperate.SapMOrderProcess, input.InspectQualified);  //质检合格数量作为交接数

                            /*
                            //准备工作
                            if (result)
                                result = await this.stepPrepare(nextSapCooperate.SapMOrderProcess, nextSapCooperate, input.InspectQualified);   

                            //SAP采购申请审批
                            if (result)
                                result = await this.stepSapPoRequestRelease(nextSapCooperate.SapMOrderProcess, nextSapCooperate);

                            //创建SAP采购订单
                            if (result)
                                result = await this.stepSapPomt(nextSapCooperate.SapMOrderProcess, nextSapCooperate);

                            if (nextSapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift)
                            {
                                //创建FS销售订单
                                if (result)
                                    result = await this.stepFsComt(context.FstiToken, nextSapCooperate.SapMOrderProcess, nextSapCooperate);

                                //创建FS生产订单
                                if (result)
                                    result = await this.stepFsMomt(context.FstiToken, nextSapCooperate.SapMOrderProcess, nextSapCooperate);

                                //创建FS发料清单（PICK）
                                if (result)
                                    result = await this.stepFsPick(context.FstiToken, nextSapCooperate.SapMOrderProcess, nextSapCooperate);
                            }
                            */
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //其他异常记录
                await this._sapMOrderProcessCooperateStepRepository.InsertAsync(new SapMOrderProcessCooperateStep
                {
                    CooperateInfo = sapCooperate,
                    StepTransactionType = SapMOrderProcessCooperateStepTransTypes.Others,
                    StepName = SapMOrderProcessCooperateStepTransTypes.Others,
                    IsStepSuccess = false,
                    StepResultMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 1800))
                });
                return false;
            }
        }

        //前序道序批量SAP入库
        private async Task<bool> sapPreviousProcessReceive(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            bool result = true;

            //当前道序之前的“已创建采购订单，且未接收入库”的外协道序
            var previousCooper = await this._sapMOrderProcessCooperateRepository.GetAll()
                .Include(co => co.SapMOrderProcess)
                .Where(co => co.SapMOrderProcess.SapMOrderId == sapProcess.SapMOrderId
                    && string.Compare(co.SapMOrderProcess.OperationNumber, sapProcess.OperationNumber, StringComparison.Ordinal) < 0
                    && (co.IsSapPomtFinished != null && co.IsSapPomtFinished.Value == true)
                    && (co.IsSapPorvFinished == null || co.IsSapPorvFinished.Value == false))
                .OrderBy(co => co.SapMOrderProcess.OperationNumber)
                .ToListAsync();

            foreach (var cooper in previousCooper)
            {
                //SAP采购订单审批
                if (result)
                    result = await this.stepSapPoRelease(cooper.SapMOrderProcess, cooper);

                //采购入库
                if (result)
                    result = await this.stepSapPorv(cooper.SapMOrderProcess, cooper);
            }
            return result;
        }

        //准备工作
        private async Task<bool> stepPrepare(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate, decimal handOverQuantity)
        {
            if (sapCooperate.IsPrepareFinished != null && sapCooperate.IsPrepareFinished.Value)
                return true;

            if (handOverQuantity <= 0)
                throw new ArgumentOutOfRangeException($"交接数量[{handOverQuantity}]异常！");

            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //交接数量
                sapCooperate.HandOverQuantity = handOverQuantity;

                if (sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToOutsideSupplier
                    && sapCooperate.CooperaterCode != _cooperateConfigurations.Cooperate_SapXcSupplierCode    //新场不需要维护道序价格（从FS物料成本计算）
                    && sapProcess.ProcessPrice <= 0)
                    throw new DomainException($"工序{sapProcess.SapMOrder.OrderNumber}/{sapProcess.OperationNumber}外协价格未写入！");

                //工艺映射及成本获取
                if (sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift
                    || sapCooperate.CooperaterCode == _cooperateConfigurations.Cooperate_SapXcSupplierCode)
                {
                    //东厂区(及新场)从对应工艺获取
                    var processMap = await this._processCodeMap
                        .FirstOrDefaultAsync(m => m.SapProcessCode == sapProcess.WorkCenterCode);
                    if (string.IsNullOrWhiteSpace(processMap?.FsAuxiProcessCode) ||
                        string.IsNullOrWhiteSpace(processMap.FsWorkProcessCode))
                        throw new DomainException($"找不到SAP工艺[{sapProcess.WorkCenterCode}]的FS映射工艺，请检查配置表");

                    sapCooperate.FsAuxiProcessCode = processMap.FsAuxiProcessCode;
                    sapCooperate.FsWorkProcessCode = processMap.FsWorkProcessCode;
                    sapCooperate.CooperaterPrice = this.getCooperProcessCost(sapProcess, sapCooperate); //0套成本

                    Logger.Info($"[{sapProcess.SapMOrder.OrderNumber}/{sapProcess.OperationNumber}]:FS Cost:{sapCooperate.CooperaterPrice}");
                }
                else
                {
                    //外协供应商获取订单道序的价格                   
                    sapCooperate.CooperaterPrice = sapCooperate.CooperaterPrice <= 0 ? sapProcess.ProcessPrice : sapCooperate.CooperaterPrice;
                    Logger.Info($"[{sapProcess.SapMOrder.OrderNumber}/{sapProcess.OperationNumber}]:SAPProcess Cost:{sapProcess.ProcessPrice}");
                }

                return await Task.FromResult(StepResult.Success);
            },
            SapMOrderProcessCooperateStepTransTypes.ProcessPrepare, SapMOrderProcessCooperateStepTransTypes.ProcessPrepare, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsPrepareFinished = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //SAP采购请求批准
        private async Task<bool> stepSapPoRequestRelease(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsSapPoRequestReleased != null && sapCooperate.IsSapPoRequestReleased.Value)
                return true;

            //执行接口
            string sapPoRequestNumber = sapProcess.BANFN;
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                if (string.IsNullOrWhiteSpace(sapPoRequestNumber))
                    throw new DomainException("该道序采购申请单号为空!");

                PoRequestReleaseInput input = new PoRequestReleaseInput
                {
                    RequestNumber = sapPoRequestNumber,
                    RelCode = _cooperateConfigurations.Cooperate_SapPoRequestReleaseRelCode,
                };

                var bapiResult = _bapiRepository.PurcharseOrderRequestRelease(input);
                return await Task.FromResult(new StepResult(bapiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.SapPoRequestRelease, SapMOrderProcessCooperateStepTransTypes.SapPoRequestRelease, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsSapPoRequestReleased = true;
                sapCooperate.SapPoRequestNumber = sapPoRequestNumber;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //创建SAP采购订单
        private async Task<bool> stepSapPomt(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsSapPomtFinished != null && sapCooperate.IsSapPomtFinished.Value)
                return true;

            //执行接口
            string sapPoNumber = sapCooperate.SapPoNumber;
            string sapPoLine = sapCooperate.SapPoLine;
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //SAP采购订单创建参数构建
                PoCreateInput input = new PoCreateInput
                {
                    BSART = "NB",
                    //LIFNR = sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift
                    //    ? _cooperateConfigurations.Cooperate_EastCustomerId : sapCooperate.CooperaterCode,  //东厂代码/供应商代码
                    LIFNR = sapCooperate.CooperaterCode,
                    EKORG = _cooperateConfigurations.Cooperate_SapWestEKORG,
                    EKGRP = _cooperateConfigurations.Cooperate_SapWestEKGRP,
                    BUKRS = _cooperateConfigurations.Cooperate_SapWestBUKRS,
                    EBELP = "00010",
                    KNTTP = "F",
                    MATNR = "", //工艺外协无料号
                    TXZ01 = sapProcess.ProcessText1,
                    MENGE = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    MEINS = sapProcess.Unit,
                    EEIND = sapProcess.ScheduleFinishDate ?? DateTime.Today.AddDays(3),
                    NETPR = sapCooperate.CooperaterPrice,
                    WAERS = "CNY",
                    MATKL = _cooperateConfigurations.Cooperate_SapWestMATKL,
                    WERKS = _cooperateConfigurations.Cooperate_SapWestWERKS,
                    MWSKZ = _cooperateConfigurations.Cooperate_SapWestMWSKZ,
                    SAKTO = _cooperateConfigurations.Cooperate_SapWestSAKTO,
                    AUFNR = sapProcess.SapMOrder.OrderNumber,
                    PREQNO = sapProcess.BANFN,  //采购申请编号
                    PREQITEM = sapProcess.BNFPO,
                    WbsElement = sapProcess.SapMOrder.WBSElement
                };

                var bapiResult = _bapiRepository.PurcharseOrderCreate(new PoCreateInput[] { input });
                sapPoNumber = bapiResult.ExtensionData;
                sapPoLine = input.EBELP;
                return await Task.FromResult(new StepResult(bapiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.SapPomt, SapMOrderProcessCooperateStepTransTypes.SapPomt, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsSapPomtFinished = true;
                sapCooperate.SapPoNumber = sapPoNumber;
                sapCooperate.SapPoLine = sapPoLine;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        private async Task<bool> stepSapPomtBatch(List<SapMOrderProcessCooperate> sapCooperateList)
        {
            var sapCooperate = sapCooperateList.First();    //以第一个道序作为采购订单创建的主体
            if (sapCooperate.IsSapPomtFinished != null && sapCooperate.IsSapPomtFinished.Value)
                return true;

            //执行接口
            string sapPoNumber = sapCooperate.SapPoNumber;
            decimal quantity = this.getCooperProcessQuantity(sapCooperate.SapMOrderProcess, sapCooperate);
            List<dynamic> bapiResultData = new List<dynamic>();
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                int lineIdx = 0;
                PoCreateInput[] inputs = sapCooperateList.Select(co =>
                {
                    string sapPoLine = ((++lineIdx) * 10).ToString().PadLeft(4, '0');
                    bapiResultData.Add(new
                    {
                        Cooper = co,
                        SapPoLine = sapPoLine
                    });

                    return new PoCreateInput
                    {
                        BSART = "NB",
                        //LIFNR = sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift
                        //    ? _cooperateConfigurations.Cooperate_EastCustomerId : sapCooperate.CooperaterCode,  //东厂代码/供应商代码
                        LIFNR = co.CooperaterCode,
                        EKORG = _cooperateConfigurations.Cooperate_SapWestEKORG,
                        EKGRP = _cooperateConfigurations.Cooperate_SapWestEKGRP,
                        BUKRS = _cooperateConfigurations.Cooperate_SapWestBUKRS,
                        EBELP = sapPoLine,
                        KNTTP = "F",
                        MATNR = "", //工艺外协无料号
                        TXZ01 = co.SapMOrderProcess.ProcessText1,
                        MENGE = quantity,
                        MEINS = co.SapMOrderProcess.Unit,
                        EEIND = co.SapMOrderProcess.ScheduleFinishDate ?? DateTime.Today.AddDays(3),
                        NETPR = co.CooperaterPrice,
                        WAERS = "CNY",
                        MATKL = _cooperateConfigurations.Cooperate_SapWestMATKL,
                        WERKS = _cooperateConfigurations.Cooperate_SapWestWERKS,
                        MWSKZ = _cooperateConfigurations.Cooperate_SapWestMWSKZ,
                        SAKTO = _cooperateConfigurations.Cooperate_SapWestSAKTO,
                        AUFNR = co.SapMOrderProcess.SapMOrder.OrderNumber,
                        PREQNO = co.SapMOrderProcess.BANFN, //采购申请编号
                        PREQITEM = co.SapMOrderProcess.BNFPO,
                        WbsElement = co.SapMOrderProcess.SapMOrder.WBSElement
                    };
                }).ToArray();

                var bapiResult = _bapiRepository.PurcharseOrderCreate(inputs);
                sapPoNumber = bapiResult.ExtensionData;
                return await Task.FromResult(new StepResult(bapiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.SapPomt, SapMOrderProcessCooperateStepTransTypes.SapPomt, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                foreach (var rd in bapiResultData)
                {
                    rd.Cooper.IsSapPomtFinished = true;
                    rd.Cooper.SapPoNumber = sapPoNumber;
                    rd.Cooper.SapPoLine = rd.SapPoLine;
                    await _sapMOrderProcessCooperateRepository.UpdateAsync(rd.Cooper);
                }
            }
            return result;
        }

        //创建FS销售订单
        private async Task<bool> stepFsComt(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsComtFinished != null && sapCooperate.IsFsComtFinished.Value)
                return true;

            //COMT参数构建
            ComtAddInput comtInput = new ComtAddInput
            {
                //西厂区生产订单号+“-”+西厂区委外工序号
                CoNumber = $"{sapProcess.SapMOrder.OrderNumber}-{sapProcess.OperationNumber}",
                CustomerId = _cooperateConfigurations.Cooperate_WestCustomerId,
                ComtAddLines = new List<ComtAddInput.LineItem>
                {
                    new ComtAddInput.LineItem
                    {
                        CoLineNumber = 1,
                        ItemNumber = _cooperateConfigurations.Cooperate_WestCooperateItemNumber,
                        ItemOrderedQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                        PromisedShipDate = this.getPromisedDate(sapProcess),
                        CoLineStatus = 4,
                        ItemControllingNetUnitPrice = sapCooperate.CooperaterPrice,
                        TextLine1 = sapCooperate.SapPoNumber,   //SAP中采购订单号
                        TextLine2 = "1",    //SAP中采购订单行号
                    }
                }
            };

            //FsComt_ADDHeader
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                var fstiResult = _fstiRepository.ComtAddHeader(fstiToken, comtInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsComt, SapMOrderProcessCooperateStepTransTypes.FsComt_ADDHeader, sapCooperate);

            //FsComt_ADDLine
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.ComtAddLines(fstiToken, comtInput).First();
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsComt, SapMOrderProcessCooperateStepTransTypes.FsComt_ADDLine, sapCooperate);
            }

            //FsComt_ADDLineText
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.ComtAddLineTexts(fstiToken, comtInput).First();
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsComt, SapMOrderProcessCooperateStepTransTypes.FsComt_ADDLineText, sapCooperate);
            }

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsComtFinished = true;
                sapCooperate.FsCoNumber = comtInput.CoNumber;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //创建FS生产订单
        private async Task<bool> stepFsMomt(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsMomtFinished != null && sapCooperate.IsFsMomtFinished.Value)
                return true;

            //COMT参数构建
            MomtAddInput momtInput = new MomtAddInput
            {
                //西厂区生产订单号+“-”+西厂区委外工序号
                MoNumber = $"{sapProcess.SapMOrder.OrderNumber}-{sapProcess.OperationNumber}",
                Planner = _cooperateConfigurations.Cooperate_Planner,
                WorkCenter = _cooperateConfigurations.Cooperate_WorkCenter,
                DeliverTo = "",
                MomtAddLines = new List<MomtAddInput.LineItem>
                {
                    new MomtAddInput.LineItem
                    {
                        ItemNumber = _cooperateConfigurations.Cooperate_WestCooperateItemNumber,
                        MoLineType = "M",
                        ItemOrderedQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                        MoLineStatus = 4,
                        StartDate = DateTime.Today,
                        ScheduledDate = this.getPromisedDate(sapProcess),

                        MoLineNumber = 1,
                        TextLine1 = sapProcess.SapMOrder.OrderNumber,   //西厂区生产订单号
                        TextLine2 = sapProcess.SapMOrder.MaterialNumber,    //西厂区生产订单物料编码
                    }
                }
            };

            //FsMomt_ADDHeader
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                var fstiResult = _fstiRepository.MomtAddHeader(fstiToken, momtInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsMomt, SapMOrderProcessCooperateStepTransTypes.FsMomt_ADDHeader, sapCooperate);

            //FsMomt_ADDLine
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.MomtAddLines(fstiToken, momtInput).First();
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsMomt, SapMOrderProcessCooperateStepTransTypes.FsMomt_ADDLine, sapCooperate);
            }

            //FsMomt_ADDLineText
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.MomtAddLineTexts(fstiToken, momtInput).First();
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsMomt, SapMOrderProcessCooperateStepTransTypes.FsMomt_ADDLineText, sapCooperate);
            }

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsMomtFinished = true;
                sapCooperate.FsMoNumber = momtInput.MoNumber;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //创建FS发料清单（PICK）
        private async Task<bool> stepFsPick(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsPickFinished != null && sapCooperate.IsFsPickFinished.Value)
                return true;

            //PICK参数构建
            PickInput pickInput = new PickInput
            {
                OrderType = "M",
                IssueType = "I",
                OrderNumber = sapCooperate.FsMoNumber,
                LineNumber = 1,
                ComponentLineType = "R",
                PointOfUseId = sapCooperate.CooperaterFsPointOfUse, //东厂区使用点
                OperationSequenceNumber = "001",
                ItemNumber = "",        //根据准备/操作动态赋值
                RequiredQuantity = 0m,  //根据准备/操作动态赋值
            };

            //FsPick_ADD_Auxi
            pickInput.ItemNumber = sapCooperate.FsAuxiProcessCode;        //准备工艺
            pickInput.RequiredQuantity = this.getAuxiTime(sapProcess);  //准备工时
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                var fstiResult = _fstiRepository.PickAdd(fstiToken, pickInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsPick, SapMOrderProcessCooperateStepTransTypes.FsPick_ADD_Auxi, sapCooperate);

            //FsPick_ADD_Work
            pickInput.ItemNumber = sapCooperate.FsWorkProcessCode;                            //操作工艺
            decimal quantity = this.getCooperProcessQuantity(sapProcess, sapCooperate);
            pickInput.RequiredQuantity = this.getWorkTime(sapProcess) * quantity;           //操作总工时
            pickInput.QuantityType = "I";                                                   //操作工艺量类为I
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.PickAdd(fstiToken, pickInput);
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsPick, SapMOrderProcessCooperateStepTransTypes.FsPick_ADD_Work, sapCooperate);
            }

            //FsPick_EDITDetail_Work
            if (result)
            {
                result = await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    var fstiResult = _fstiRepository.PickEdit(fstiToken, pickInput);
                    return await Task.FromResult(new StepResult(fstiResult));
                },
                SapMOrderProcessCooperateStepTransTypes.FsPick, SapMOrderProcessCooperateStepTransTypes.FsPick_EDITDetail_Work, sapCooperate);
            }

            //更新FS Pick到可视化MES（失败不影响交接流程）
            if (result)
            {
                await this.trySapMOrderProcessCooperateStep(async () =>
                {
                    _vmesRepository.SyncPickToVmes(new SyncPickToVmesInput
                    {
                        MONumber = pickInput.OrderNumber,
                        MOLineNumber = pickInput.LineNumber
                    });
                    return await Task.FromResult(StepResult.Success);
                },
                SapMOrderProcessCooperateStepTransTypes.FsPick, SapMOrderProcessCooperateStepTransTypes.FsPick_SyncToMes, sapCooperate);
            }

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsPickFinished = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //FS入库送检
        private async Task<bool> stepFsMorv(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsMorvFinished != null && sapCooperate.IsFsMorvFinished.Value)
                return true;

            MorvInput morvInput = null;
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //MORV参数构建
                morvInput = new MorvInput
                {
                    MoNumber = sapCooperate.FsMoNumber,
                    MoLineNumber = 1,
                    ItemNumber = _cooperateConfigurations.Cooperate_WestCooperateItemNumber,
                    ReceiptQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    StockRoom = _cooperateConfigurations.Cooperate_FsMoStockRoom,
                    Bin = _cooperateConfigurations.Cooperate_FsMoStockBin,
                    InventoryCategory = "I",
                    LotNumber = ""  //留空
                };

                var fstiResult = _fstiRepository.Morv(fstiToken, morvInput);
                morvInput.LotNumber = fstiResult.ExtensionData;
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsMorv, SapMOrderProcessCooperateStepTransTypes.FsMorv, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsMorvFinished = true;
                sapCooperate.LotNumber = morvInput.LotNumber;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //FS验收移库
        private async Task<bool> stepFsImtr(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsImtrFinished != null && sapCooperate.IsFsImtrFinished.Value)
                return true;

            ImtrInput imtrInput = null;
            string documentPrefix = $"XC-{DateTime.Today.ToString("yyMM")}";
            int documentSerial = 0;
            string documentNumberFull = null;   //“XC-YYMM”+3位流水号
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //IMTR参数构建
                documentSerial = await this.getImtrDocumentNumberSerial(documentPrefix);
                documentNumberFull = $"{documentPrefix}{documentSerial:D3}";
                string itemNumber = _cooperateConfigurations.Cooperate_WestCooperateItemNumber;
                FSItem fsItem = this.getFsItem(itemNumber);
                imtrInput = new ImtrInput
                {
                    ItemNumber = itemNumber,
                    StockroomFrom = _cooperateConfigurations.Cooperate_FsMoStockRoom,
                    BinFrom = _cooperateConfigurations.Cooperate_FsMoStockBin,
                    InventoryCategoryFrom = "I",
                    InventoryQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    StockroomTo = fsItem?.PreferredStockroom,   //优选库位
                    BinTo = fsItem?.PreferredBin,               //优选库位
                    InventoryCategoryTo = "O",
                    LotNumber = sapCooperate.LotNumber,
                    OrderType = "M",
                    OrderNumber = sapCooperate.FsMoNumber,
                    LineNumber = 1,
                    DocumentNumber = documentNumberFull,
                };

                var fstiResult = _fstiRepository.Imtr(fstiToken, imtrInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsImtr, SapMOrderProcessCooperateStepTransTypes.FsImtr, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsImtrFinished = true;
                sapCooperate.ImtrDocumentNumberPrefix = documentPrefix;
                sapCooperate.ImtrDocumentNumberSerialNumber = documentSerial;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //FS移销售库
        private async Task<bool> stepFsImtrSales(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsImtrSalesFinished != null && sapCooperate.IsFsImtrSalesFinished.Value)
                return true;

            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //IMTR参数构建
                string itemNumber = _cooperateConfigurations.Cooperate_WestCooperateItemNumber;
                FSItem fsItem = this.getFsItem(itemNumber);
                ImtrInput imtrInput = new ImtrInput
                {
                    ItemNumber = itemNumber,
                    StockroomFrom = fsItem?.PreferredStockroom, //优选库位
                    BinFrom = fsItem?.PreferredBin,             //优选库位
                    InventoryCategoryFrom = "O",
                    InventoryQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    StockroomTo = _cooperateConfigurations.Cooperate_FsShipStockRoom,
                    BinTo = _cooperateConfigurations.Cooperate_FsShipStockBin,
                    InventoryCategoryTo = "S",
                    LotNumber = sapCooperate.LotNumber,
                    OrderType = "C",
                    OrderNumber = sapCooperate.FsCoNumber,
                    LineNumber = 1,
                    DocumentNumber = sapCooperate.ImtrDocumentNumber,
                };

                var fstiResult = _fstiRepository.Imtr(fstiToken, imtrInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsImtrSales, SapMOrderProcessCooperateStepTransTypes.FsImtrSales, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsImtrSalesFinished = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //FS发运客户
        private async Task<bool> stepFsShip(FstiToken fstiToken, SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsFsShipFinished != null && sapCooperate.IsFsShipFinished.Value)
                return true;

            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //SHIP参数构建
                ShipInput shipInput = new ShipInput
                {
                    CoNumber = sapCooperate.FsCoNumber,
                    CoLineNumber = 1,
                    ShippedQuantity = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    Stockroom = this._cooperateConfigurations.Cooperate_FsShipStockRoom,
                    Bin = this._cooperateConfigurations.Cooperate_FsShipStockBin,
                    InventoryCategory = "S",
                    LotNumber = sapCooperate.LotNumber
                };

                var fstiResult = _fstiRepository.Ship(fstiToken, shipInput);
                return await Task.FromResult(new StepResult(fstiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.FsShip, SapMOrderProcessCooperateStepTransTypes.FsShip, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsFsShipFinished = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //SAP采购单批准
        private async Task<bool> stepSapPoRelease(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsSapPoReleased != null && sapCooperate.IsSapPoReleased.Value)
                return true;

            //执行接口
            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                PoReleaseInput input = new PoReleaseInput
                {
                    PoNumber = sapCooperate.SapPoNumber,
                    RelCode = _cooperateConfigurations.Cooperate_SapPoReleaseRelCode,
                };

                var bapiResult = _bapiRepository.PurcharseOrderRelease(input);
                return await Task.FromResult(new StepResult(bapiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.SapPoRelease, SapMOrderProcessCooperateStepTransTypes.SapPoRelease, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsSapPoReleased = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //SAP采购订单入库
        private async Task<bool> stepSapPorv(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            if (sapCooperate.IsSapPorvFinished != null && sapCooperate.IsSapPorvFinished.Value)
                return true;

            bool result = await this.trySapMOrderProcessCooperateStep(async () =>
            {
                //SAP采购订单入库参数构建
                PoFinishInput input = new PoFinishInput
                {
                    MATERIAL = "",  //工艺外协无料号
                    PLANT = _cooperateConfigurations.Cooperate_SapWestWERKS,
                    MOVE_TYPE = "101",
                    STCK_TYPE = "2",
                    VENDOR = _cooperateConfigurations.Cooperate_EastCustomerId,
                    ENTRY_QNT = this.getCooperProcessQuantity(sapProcess, sapCooperate),
                    ENTRY_UOM = sapProcess.Unit,
                    MVT_IND = "B",
                    PO_NUMBER = sapCooperate.SapPoNumber,
                    PO_ITEM = sapCooperate.SapPoLine,
                };

                var bapiResult = _bapiRepository.PurcharseOrderFinish(input);
                return await Task.FromResult(new StepResult(bapiResult));
            },
            SapMOrderProcessCooperateStepTransTypes.SapPorv, SapMOrderProcessCooperateStepTransTypes.SapPorv, sapCooperate);

            //更新主表步骤状态
            if (result)
            {
                sapCooperate.IsSapPorvFinished = true;
                await _sapMOrderProcessCooperateRepository.UpdateAsync(sapCooperate);
            }
            return result;
        }

        //获取连续供应商的道序
        private async Task<List<SapMOrderProcessCooperate>> getContinuousSupplierProcess(SapMOrder sapMOrder, string sapMOrderProcessNumber, bool isAsc)
        {
            var sapProcessList = sapMOrder.OrderProcess;
            if (isAsc)
                sapProcessList = sapProcessList.OrderBy(p => p.OperationNumber).ToList();
            else
                sapProcessList = sapProcessList.OrderByDescending(p => p.OperationNumber).ToList();

            List<SapMOrderProcessCooperate> sapCooperateList = new List<SapMOrderProcessCooperate>();
            bool isPredicateProcess = false;
            string supplierCode = null;
            for (int i = 0; i < sapProcessList.Count; i++)
            {
                var currentProcess = sapProcessList[i];
                if (currentProcess.OperationNumber == sapMOrderProcessNumber)
                {
                    isPredicateProcess = true; //开始位置
                    supplierCode = currentProcess.LIFNR;    //供应商
                }
                if (isPredicateProcess)
                {
                    if (currentProcess.LIFNR == supplierCode)
                    {
                        var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                        .Include(co => co.SapMOrderProcess)
                        .FirstOrDefaultAsync(co => co.SapMOrderProcessId == currentProcess.Id);
                        if (sapCooperate != null)
                            sapCooperateList.Add(sapCooperate);
                    }
                    else
                    {
                        break;  //供应商不同，跳出
                    }
                }
            }

            if (!isAsc)
                sapCooperateList.Reverse(); //反转为道序顺序
            return sapCooperateList;
        }

        //找到需要送回的外协道序
        private async Task<List<SapMOrderProcessCooperate>> getShipBackSapCooperateList(SapMOrder sapMOrder, string sapMOrderProcessNumber)
        {
            List<SapMOrderProcessCooperate> sapCooperateList = new List<SapMOrderProcessCooperate>();
            var sapProcessList = sapMOrder.OrderProcess.OrderBy(p => p.OperationNumber).ToList();
            int endIdx = -1, startIdx = -1;
            for (int i = sapProcessList.Count - 1; i >= 0; i--)
            {
                var currentProcess = sapProcessList[i];
                if (currentProcess.OperationNumber == sapMOrderProcessNumber)
                {
                    startIdx = endIdx = i;   //外协结束道序索引
                    var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                        .Include(co => co.SapMOrderProcess)
                        .FirstOrDefaultAsync(co => co.SapMOrderProcessId == currentProcess.Id);
                    if (sapCooperate != null)
                        sapCooperateList.Add(sapCooperate); //外协结束道序
                    break;
                }
            }

            if (sapCooperateList.Count < 1)
                throw new DomainException($"工序{sapMOrder.OrderNumber}/{sapMOrderProcessNumber}外协信息不存在！");

            for (int i = endIdx; i >= 0; i--)
            {
                var currentProcess = sapProcessList[i];
                var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                    .Include(co => co.SapMOrderProcess)
                    .FirstOrDefaultAsync(co => co.SapMOrderProcessId == currentProcess.Id);
                //连续的相同使用点外协道序
                if (sapCooperate != null
                    && sapCooperate.CooperateType == sapCooperateList[0].CooperateType)
                {
                    //外协给东厂时不考虑使用点，外协给供应商时要考虑供应商编码是否相同！
                    if (sapCooperate.CooperateType == SapMOrderProcessCooperateType.ToForthShift
                        || sapCooperate.CooperaterCode == sapCooperateList[0].CooperaterCode)
                    {
                        startIdx = i;   //外协开始道序索引
                        sapCooperateList.Add(sapCooperate);
                    }
                }
                else
                {
                    break;
                }
            }

            sapCooperateList.Reverse(); //反转为道序顺序
            return sapCooperateList;
        }

        //外协准备工时
        private decimal getAuxiTime(SapMOrderProcess sapProcess)
        {
            if (sapProcess.SapMOrder.MRPController == "100")
            {
                //金切订单
                return sapProcess.VGW01;
            }
            else
            {
                //装配订单
                return sapProcess.VGW01;
            }
        }

        //外协操作工时
        private decimal getWorkTime(SapMOrderProcess sapProcess)
        {
            if (sapProcess.SapMOrder.MRPController == "100")
            {
                //金切订单
                return sapProcess.VGW02;
            }
            else
            {
                //装配订单
                return sapProcess.VGW03;
            }
        }

        //外协成本=（准备工时/10+操作工时）*数量*操作工时的0套成本
        private decimal getCooperProcessCost(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            var auxi = this.getAuxiTime(sapProcess);
            var work = this.getWorkTime(sapProcess);
            var quantity = this.getCooperProcessQuantity(sapProcess, sapCooperate);
            var workCost0 = this._fsRepository.GetItemCost0ByItemNumber(sapCooperate.FsWorkProcessCode); //操作工艺的0套成本
            return (auxi / 10m + work) * quantity * workCost0;
        }

        //外协数量计算
        private decimal getCooperProcessQuantity(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapCooperate)
        {
            ////外协FS检验合格数量
            //if (sapCooperate.IsMesInspectFinished != null && sapCooperate.IsMesInspectFinished.Value)
            //{
            //    return sapCooperate.MesInspectQualified ?? 0m;
            //}
            //return sapProcess.ProcessQuantity;
            return sapCooperate.HandOverQuantity ?? 0m;
        }

        //承诺日期=max（计划完工日期，today+3）
        private DateTime getPromisedDate(SapMOrderProcess sapProcess)
        {
            DateTime todayAdd3 = DateTime.Today.AddDays(3);
            DateTime scheduleFinishDate = sapProcess.ScheduleFinishDate ?? todayAdd3;
            DateTime promisedDate = DateTime.Compare(todayAdd3, scheduleFinishDate) >= 0 ? todayAdd3 : scheduleFinishDate;
            return promisedDate;
        }

        //取得IMTR文档号中的流水号
        private async Task<int> getImtrDocumentNumberSerial(string documentNumberPrefix)
        {
            int? currentSerial = await this._sapMOrderProcessCooperateRepository.GetAll()
                .Where(co => co.ImtrDocumentNumberPrefix == documentNumberPrefix)
                .MaxAsync(co => co.ImtrDocumentNumberSerialNumber);
            return currentSerial ?? 0 + 1;
        }

        //取得FS物料信息
        private FSItem getFsItem(string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                return null;
            return this._fsRepository.GetItemByItemNumber(itemNumber);
        }


        //执行一个过程方法，（无论成功与否）记录一条日志
        private async Task<bool> trySapMOrderProcessCooperateStep(Func<Task<StepResult>> tryFunc,
            string stepTransactionType, string stepName, SapMOrderProcessCooperate sapCooperate)
        {
            //检查该步骤是否已经成功执行过
            bool hasSucceess = await _sapMOrderProcessCooperateStepRepository.GetAll()
                .AnyAsync(step => step.SapMOrderProcessCooperateId == sapCooperate.Id
                                  && step.StepTransactionType == stepTransactionType && step.StepName == stepName
                                  && step.IsStepSuccess);
            if (hasSucceess)
                return true;

            //执行该步骤
            StepResult result = StepResult.Failure;
            try
            {
                result = await tryFunc();
                return result.IsSuccess;
            }
            catch (Exception ex)
            {
                result = new StepResult(false, ex.Message);
                return false;
            }
            finally
            {
                //步骤记录
                await this._sapMOrderProcessCooperateStepRepository.InsertAsync(new SapMOrderProcessCooperateStep
                {
                    CooperateInfo = sapCooperate,
                    StepTransactionType = stepTransactionType,
                    StepName = stepName,
                    IsStepSuccess = result.IsSuccess,
                    StepResultMessage = result.ResultMessage.Substring(0, Math.Min(result.ResultMessage.Length, 1800))
                });
            }
        }

        /// <summary>
        /// 取得SAP订单中外协工艺的接口日志
        /// </summary>
        /// <param name="sapMOrderNumber">SAP制造订单号</param>
        /// <returns>日志列表</returns>
        public async Task<ListResultDto<SapCooperProcessLogOutput>> GetSapMOrderCooperLogs(string sapMOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(sapMOrderNumber))
                throw new ArgumentNullException("sapMOrderNumber");

            var sapProcessWithCooperateList = await this._sapMOrderManager.GetSapMOrderProcessListWithCooperateQuery()
                .Where(p => p.ProcessLine.SapMOrder.OrderNumber == sapMOrderNumber && p.CooperateLine != null)
                .OrderBy(p => p.ProcessLine.OperationNumber)
                .ToListAsync(); //仅外协道序

            var logList = new List<SapCooperProcessLogOutput>();
            foreach (var sapProcessWithCooperate in sapProcessWithCooperateList)
            {
                var logItem = sapProcessWithCooperate.CooperateLine.MapTo<SapCooperProcessLogOutput>();
                var sapProcess = sapProcessWithCooperate.ProcessLine;
                var sapOrder = sapProcess.SapMOrder;

                //订单信息
                logItem.OrderNumber = sapOrder.OrderNumber;
                logItem.MaterialNumber = sapOrder.MaterialNumber;
                logItem.MaterialDescription = sapOrder.MaterialDescription;
                logItem.TargetQuantity = sapOrder.TargetQuantity;
                //工艺信息
                logItem.OperationNumber = sapProcess.OperationNumber;
                logItem.WorkCenterCode = sapProcess.WorkCenterCode;
                logItem.WorkCenterName = sapProcess.WorkCenterName;

                logList.Add(logItem);
            }
            return new ListResultDto<SapCooperProcessLogOutput>(logList);
        }

        /// <summary>
        /// 步骤执行结果
        /// </summary>
        private class StepResult
        {
            /// <summary>
            /// 是否执行成功
            /// </summary>
            public bool IsSuccess { get; set; }
            /// <summary>
            /// 执行反馈消息
            /// </summary>
            public string ResultMessage { get; set; }

            public StepResult(bool isSuccess, string message = "")
            {
                this.IsSuccess = isSuccess;
                this.ResultMessage = message;
            }

            public StepResult(FstiResult fstiResult)
            {
                this.IsSuccess = fstiResult.IsSuccess;
                this.ResultMessage = fstiResult.Message;
            }

            public StepResult(BapiResult bapiResult)
            {
                this.IsSuccess = bapiResult.IsSuccess;
                this.ResultMessage = bapiResult.Message;
            }

            public static readonly StepResult Failure = new StepResult(false);

            public static readonly StepResult Success = new StepResult(true);
        }
    }
}
