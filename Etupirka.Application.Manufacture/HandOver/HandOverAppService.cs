using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using Etupirka.Application.Manufacture.Cooperate;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Etupirka.Application.Manufacture.HandOver.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.External.Entities.Vmes;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Manufacture.Services;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.HandOver
{
    /// <summary>
    /// 交接单管理
    /// </summary>
    [AbpAuthorize]
    public class HandOverAppService : EtupirkaAppServiceBase, IHandOverAppService
    {
        private readonly IRepository<HandOverBill, int> _handOverBillRepository;
        private readonly IRepository<HandOverBillLine, int> _handOverBillLineRepository;
        private readonly IRepository<SapMOrder, Guid> _sapMOrderRepository;
        private readonly IRepository<SapMOrderProcess, Guid> _sapMOrderProcessRepository;
        private readonly IRepository<SapMOrderProcessCooperate, int> _sapMOrderProcessCooperateRepository;
        private readonly HandOverBillFactory _handOverBillFactory;
        private readonly HandOverSourceManager _handOverSourceManager;
        private readonly SapMOrderManager _sapMOrderManager;
        private readonly ICooperateAppService _cooperateAppService;
        private readonly IBAPIRepository _bapiRepository;
        private readonly IVMESRepository _vmesRepository;

        public HandOverAppService(
            IRepository<HandOverBill, int> handOverBillRepository,
            IRepository<HandOverBillLine, int> handOverBillLineRepository,
            IRepository<SapMOrder, Guid> sapMOrderRepository,
            IRepository<SapMOrderProcess, Guid> sapMOrderProcessRepository,
            IRepository<SapMOrderProcessCooperate, int> sapMOrderProcessCooperateRepository,
            HandOverBillFactory handOverBillFactory,
            HandOverSourceManager handOverSourceManager,
            SapMOrderManager sapMOrderManager,
            ICooperateAppService cooperateAppService,
            IBAPIRepository bapiRepository,
            IVMESRepository vmesRepository)
        {
            this._handOverBillRepository = handOverBillRepository;
            this._handOverBillLineRepository = handOverBillLineRepository;
            this._sapMOrderRepository = sapMOrderRepository;
            this._sapMOrderProcessRepository = sapMOrderProcessRepository;
            this._sapMOrderProcessCooperateRepository = sapMOrderProcessCooperateRepository;
            this._handOverBillFactory = handOverBillFactory;
            this._handOverSourceManager = handOverSourceManager;
            this._sapMOrderManager = sapMOrderManager;
            this._cooperateAppService = cooperateAppService;
            this._bapiRepository = bapiRepository;
            this._vmesRepository = vmesRepository;
        }

        #region 交接部门

        /// <summary>
        /// 取得所有可交接部门
        /// </summary>
        public async Task<ListResultDto<HandOverDepartmentDto>> GetAllHandOverDepartments()
        {
            var list = await this._handOverSourceManager.GetAllHandOverDepartments();
            return new ListResultDto<HandOverDepartmentDto>(list.MapTo<List<HandOverDepartmentDto>>());
        }

        #endregion

        #region 交接单

        /// <summary>
        /// 获取交接单
        /// </summary>
        public async Task<HandOverBillOutput> GetHandOverBill(int id)
        {
            var bill = await this._handOverBillRepository.FirstOrDefaultAsync(id);
            return bill.MapTo<HandOverBillOutput>();
        }

        /// <summary>
        /// 查询交接单（带交接行统计）
        /// </summary>
        public async Task<PagedResultDto<HandOverBillWithLineStatisticsOutput>> FindHandOverBills(FindHandOverBillsInput input)
        {

            var query = _handOverBillRepository.GetAll()
                .WhereIf(input.State != null, b => input.State.Contains(b.BillState))
                .WhereIf(input.RangeBegin != null, b => b.CreationTime >= input.RangeBegin)
                .WhereIf(input.RangeEnd != null, b => b.CreationTime <= input.RangeEnd)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BillCode), b => input.BillCode == b.BillCodePrefix + b.BillCodeSerialNumber)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransferSourceName), b => b.TransferSource.OrganizationUnitName.Contains(input.TransferSourceName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransferTargetName), b =>
                    b.TransferTargetDepartment.OrganizationUnitName.Contains(input.TransferTargetName)
                    || b.TransferTargetSupplier.SupplierCode == input.TransferTargetName
                    || b.TransferTargetSupplier.SupplierName.Contains(input.TransferTargetName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemNumber), b => b.BillLines.Any(l => l.ItemNumber == input.ItemNumber))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNumber), b => b.BillLines.Any(l => l.OrderInfo.OrderNumber == input.OrderNumber))
                .Select(b => new
                {
                    HandOverBill = b,
                    TotalLineCount = b.BillLines.Count,
                    PendingLineCount = b.BillLines.Count(l => l.LineState == HandOverBillLineState.Pending),
                    ReceivedLineCount = b.BillLines.Count(l => l.LineState == HandOverBillLineState.Received),
                    RejectedLineCount = b.BillLines.Count(l => l.LineState == HandOverBillLineState.Rejected),
                    TransferLineCount = b.BillLines.Count(l => l.LineState == HandOverBillLineState.Transfer)
                });

            var count = await query.CountAsync();
            var bills = await query
                .OrderBy(b => b.HandOverBill.BillState)
                .ThenByDescending(b => b.HandOverBill.BillCodePrefix)
                .ThenByDescending(b => b.HandOverBill.BillCodeSerialNumber)
                .PageBy(input).ToListAsync();

            return new PagedResultDto<HandOverBillWithLineStatisticsOutput>(count,
                bills.Select(b =>
                {
                    var dto = b.HandOverBill.MapTo<HandOverBillWithLineStatisticsOutput>();
                    dto.TotalLineCount = b.TotalLineCount;
                    dto.PendingLineCount = b.PendingLineCount;
                    dto.ReceivedLineCount = b.ReceivedLineCount;
                    dto.RejectedLineCount = b.RejectedLineCount;
                    dto.TransferLineCount = b.TransferLineCount;
                    return dto;
                }).ToList());
        }

        /// <summary>
        /// 创建一张交接单
        /// </summary>
        public async Task<HandOverBillOutput> CreateHandOverBill()
        {
            var currentUser = await this.GetCurrentUserAsync();
            var currentDepartment = await this.GetCurrentUserOrganizationUnitAsync();

            HandOverBill handOverBill = await this._handOverBillFactory.CreateHandOverBill(currentUser, currentDepartment);
            await this._handOverBillRepository.InsertAndGetIdAsync(handOverBill);
            return handOverBill.MapTo<HandOverBillOutput>();
        }

        /// <summary>
        /// 保存交接单信息
        /// </summary>
        public async Task SaveHandOverBill(SaveHandOverBillInput input)
        {
            var bill = await this.saveHandOverBill(input);
            await this._handOverBillRepository.UpdateAsync(bill);
        }

        /// <summary>
        /// 发布交接单
        /// </summary>
        public async Task<bool> PublishHandOverBill(SaveHandOverBillInput input)
        {
            var bill = await this.saveHandOverBill(input);

            if (bill.TransferTargetType == HandOverTargetType.Department
                && bill.TransferTargetDepartment.OrganizationUnitId == null)
            {
                throw new UserFriendlyException("部门交接单转入部门不可为空！");
            }
            else if (bill.TransferTargetType == HandOverTargetType.Supplier
                && bill.TransferTargetSupplier.SupplierCode == null)
            {
                throw new UserFriendlyException("供方交接单转入供方代码不可为空！");
            }

            if (!bill.BillLines.Any())
            {
                throw new UserFriendlyException("请添加交接物料！");
            }

            //判断所有道序是否都已经SAP质检
            bool isAllInspected = true;
            foreach (var billLine in bill.BillLines)
            {
                if (billLine.CurrentProcess == null || billLine.CurrentProcess.IsEmpty())
                    continue;   //首个道序交接
                if (billLine.InspectState == HandOverBillLineInspectState.Inspected)
                    continue;   //已检验，不用再调用接口检查

                try
                {
                    bool isInspected = await this.isHandOverLineInspected(billLine.OrderInfo.OrderNumber, billLine.CurrentProcess.ProcessNumber);
                    billLine.InspectState = isInspected ? HandOverBillLineInspectState.Inspected : HandOverBillLineInspectState.UnInspected;
                    billLine.InspectStateErrorMessage = null;

                    if (!isInspected)
                    {
                        isAllInspected = false;
                    }
                }
                catch (Exception ex)
                {
                    isAllInspected = false;
                    billLine.InspectState = HandOverBillLineInspectState.Error;
                    billLine.InspectStateErrorMessage = ex.Message;
                }

                await this._handOverBillLineRepository.UpdateAsync(billLine); //更新检验状态
            }

            //是否所有交接行都已经SAP检验
            if (isAllInspected)
            {
                bill.HandOverDate = DateTime.Now;
                bill.BillState = HandOverBillState.Published;
                await this._handOverBillRepository.UpdateAsync(bill);
                return true; //发布成功
            }
            else
            {
                throw new UserFriendlyException("存在未SAP质检的交接单行！");
            }
        }

        //验证是否已经质检：SAP质检、或MES质检...
        private async Task<bool> isHandOverLineInspected(string orderNumber, string operationSeqn)
        {
            //调用SAP接口判断道序检验情况
            var sapInspectOutput = _bapiRepository.GetSapMoInspectState(new GetSapMoInspectStateInput
            {
                MOrderNumber = orderNumber,
                OperationSeqnNumber = operationSeqn,
            });
            if (sapInspectOutput.IsInspected())
            {
                return true;
            }

            //判断VMES是否道序质检过
            var sapProcess = await this._sapMOrderManager.GetSapMOrderProcess(orderNumber, operationSeqn);
            if (sapProcess.CooperateLine != null)
            {
                var isVmesInspected = _vmesRepository.IsInspected(new IsInspectedInput
                {
                    MONumber = sapProcess.CooperateLine.FsMoNumber,
                    MOLineNumber = 1,
                    ProcessNumber = "1"
                });
                if (isVmesInspected)
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<HandOverBill> saveHandOverBill(SaveHandOverBillInput input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            var currentBill = await this._handOverBillRepository.FirstOrDefaultAsync(input.Id);
            if (currentBill == null)
                throw new DomainException($"交接单[{input.Id}]不存在！");
            if (currentBill.BillState > HandOverBillState.Draft)
                throw new UserFriendlyException($"交接单[{input.Id}]状态为[{currentBill.BillState.GetDescription()}]，不可修改！");

            input.MapTo(currentBill);
            if (currentBill.TransferTargetType == HandOverTargetType.Department)
                currentBill.TransferTargetSupplier?.Clear();
            else if (currentBill.TransferTargetType == HandOverTargetType.Supplier)
                currentBill.TransferTargetDepartment?.Clear();

            return currentBill;
        }

        /// <summary>
        /// 删除交接单
        /// </summary>
        public async Task DeleteHandOverBill(int id)
        {
            var currentBill = await this._handOverBillRepository.FirstOrDefaultAsync(id);
            if (currentBill == null)
                throw new DomainException($"交接单[{id}]不存在！");
            if (currentBill.BillState > HandOverBillState.Draft)
                throw new UserFriendlyException($"交接单[{id}]状态为[{currentBill.BillState.GetDescription()}]，不可删除！");

            //TODO:删除交接单行
            await this._handOverBillRepository.DeleteAsync(currentBill);
        }

        #endregion

        #region 交接单行

        /// <summary>
        /// 取得交接单明细行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        public async Task<ListResultDto<HandOverBillLineOutput>> GetHandOverBillLines(int billId)
        {
            var bill = await this._handOverBillRepository.GetAll()
                .Include(b => b.BillLines)
                .FirstAsync(b => b.Id == billId);

            return new ListResultDto<HandOverBillLineOutput>(bill.BillLines.MapTo<List<HandOverBillLineOutput>>());
        }

        /// <summary>
        /// 添加SAP交接单行
        /// </summary>
        public async Task AddSapHandOverBillLine(AddSapHandOverBillLineInput input)
        {
            var bill = await this._handOverBillRepository.GetAll()
                .FirstAsync(b => b.Id == input.BillId);

            if (bill.BillState > HandOverBillState.Draft)
                throw new UserFriendlyException($"交接单[{input.BillId}]状态为[{bill.BillState.GetDescription()}]，不可修改！");

            if (input.HandOverQuantity <= 0)
                throw new UserFriendlyException($"交接数量必须大于0！");

            SapMOrder sapOrder = await this._sapMOrderRepository.FirstOrDefaultAsync(input.SapMOrderId);
            if (sapOrder == null)
                throw new DomainException($"订单[{input.SapMOrderId}]不存在！");

            //取得SAP道序（含外协信息）
            SapMOrderProcess sapProcess = null; //本道序
            SapMOrderProcessCooperate sapProcessCooperate = null;
            SapMOrderProcess sapNextProcess = null; //下道序
            SapMOrderProcessCooperate sapNextProcessCooperate = null;
            if (input.SapMOrderProcessId == null)
            {
                //首道序交接
                var sapFirstProcessWithCooperate = await this._sapMOrderManager.GetFirstSapMOrderProcess(sapOrder.Id);
                sapNextProcess = sapFirstProcessWithCooperate?.ProcessLine;
                sapNextProcessCooperate = sapFirstProcessWithCooperate?.CooperateLine;
            }
            else
            {
                //非首道序交接
                var sapProcessWithCooperate = await this._sapMOrderManager.GetSapMOrderProcessListWithCooperateQuery()
                    .Where(p => p.ProcessLine.Id == input.SapMOrderProcessId)
                    .FirstOrDefaultAsync();
                if (sapProcessWithCooperate == null)
                    throw new DomainException($"SAP道序[{input.SapMOrderProcessId}]不存在！");

                sapProcess = sapProcessWithCooperate.ProcessLine;
                sapProcessCooperate = sapProcessWithCooperate.CooperateLine;    //外协信息(只有东厂往西厂还的时候才有)

                var next = await this._sapMOrderManager.GetNextSapMOrderProcess(sapProcess.Id);
                sapNextProcess = next?.ProcessLine;
                sapNextProcessCooperate = next?.CooperateLine;
            }

            //验证是否重复添加该SAP订单
            bool exists = await this._handOverBillLineRepository.GetAll()
               .AnyAsync(bl => bl.HandOverBillId == input.BillId
                    && bl.OrderInfo.OrderNumber == sapOrder.OrderNumber);
            if (exists)
                throw new UserFriendlyException($"SAP订单[{sapOrder.OrderNumber}]已添加交接，请勿重复添加！");

            //TODO:验证道序是否可交接(根据转入转出部门)

            //构建交接行
            HandOverBillLine billLine = new HandOverBillLine
            {
                HandOverBillId = bill.Id,
                HandOverBill = bill,
                OrderInfo = OrderInfo.CreateFromSap(sapOrder),
                ItemNumber = sapOrder.MaterialNumber,
                DrawingNumber = string.Empty,
                ItemDescription = sapOrder.MaterialDescription,
                HandOverQuantity = input.HandOverQuantity,
                CurrentProcess = OrderProcess.Empty(),
                NextProcess = OrderProcess.Empty(),
                Remark = input.Remark ?? "",
                LineState = HandOverBillLineState.Pending
            };
            if (sapProcess != null)
            {
                billLine.CurrentProcess = OrderProcess.CreateFromSap(sapProcess, sapProcessCooperate);
            }
            if (sapNextProcess != null)
            {
                billLine.NextProcess = OrderProcess.CreateFromSap(sapNextProcess, sapNextProcessCooperate);
            }
            await this._handOverBillLineRepository.InsertAndGetIdAsync(billLine);
        }


        /// <summary>
        /// 删除交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        public async Task DeleteHandOverBillLines(int billId, int[] lineIds)
        {
            var bill = await this._handOverBillRepository.GetAll()
                //.Include(b => b.BillLines)
                .FirstAsync(b => b.Id == billId);

            if (bill.BillState > HandOverBillState.Draft)
                throw new UserFriendlyException($"交接单[{billId}]状态为[{bill.BillState.GetDescription()}]，不可修改！");

            var billLines = bill.BillLines.Where(l => lineIds.Contains(l.Id)).ToList();
            foreach (var line in billLines)
            {
                if (line.LineState != HandOverBillLineState.Pending)
                    continue;

                await this._handOverBillLineRepository.DeleteAsync(line);
            }
        }

        /// <summary>
        /// 接收选中交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        /// <returns>交接单是否全部完成</returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task<bool> ReceiveHandOverBillLines(int billId, int[] lineIds)
        {
            var currentUser = await this.GetCurrentUserAsync();

            List<int> receivedBillLineIds = new List<int>();    //接口执行成功的交接单行
            List<dynamic> createCooperateParamList = new List<dynamic>();   //交接单行外协送出接口相关参数列表
            using (var uow = this.UnitOfWorkManager.Begin())
            {
                var bill = await this._handOverBillRepository.GetAll()
                    .Include(b => b.BillLines)
                    .FirstAsync(b => b.Id == billId);

                if (bill.BillState != HandOverBillState.Published)
                    throw new UserFriendlyException($"交接单[{billId}]状态为[{bill.BillState.GetDescription()}]，不可执行接收操作！");

                var billLines = bill.BillLines.Where(l => lineIds.Contains(l.Id)).ToList();
                foreach (var line in billLines)
                {
                    if (line.LineState != HandOverBillLineState.Pending)
                        continue;

                    if (line.OrderInfo.SourceName == OrderSourceNames.SAP)
                    {
                        //SAP订单交接行, 构建对应接口参数
                        if (line.IsSapSendOut() && !line.NextProcess.IsEmpty())
                        {
                            createCooperateParamList.Add(new
                            {
                                LineId = line.Id,
                                CreateErpOrdersInput = new SapCooperSendInput
                                {
                                    SapMOrderNumber = line.OrderInfo.OrderNumber,
                                    SapMOrderProcessNumber = line.NextProcess.ProcessNumber, /*SAP送出时，执行下个道序相关接口*/
                                    Direction = SapCooperSendInput.SapCooperSendDirection.SendOut,
                                    HandOverQuantity = line.HandOverQuantity,
                                }
                            });
                        }
                        //else if (line.IsSapSendBack())
                        //{
                        //    createCooperateParamList.Add(new
                        //    {
                        //        LineId = line.Id,
                        //        CreateErpOrdersInput = new SapCooperSendInput
                        //        {
                        //            SapMOrderNumber = line.OrderInfo.OrderNumber,
                        //            SapMOrderProcessNumber = line.CurrentProcess.ProcessNumber, /*SAP送回时，执行当前道序（以及之前所有本次外协道序）相关接口*/
                        //            Direction = SapCooperSendInput.SapCooperSendDirection.SendBack,
                        //            HandOverQuantity = line.HandOverQuantity,
                        //        }
                        //    });
                        //}
                        else
                        {
                            //直接标识为成功
                            receivedBillLineIds.Add(line.Id);
                        }
                    }
                    else
                    {
                        //非SAP订单交接行, 直接标识为成功
                        receivedBillLineIds.Add(line.Id);
                    }
                }
                uow.Complete();
            }

            //执行SAP外协接口（送出/送回）
            foreach (var param in createCooperateParamList)
            {
                try
                {
                    bool result = true;
                    SapCooperSendInput cooperInput = param.CreateErpOrdersInput;
                    if (cooperInput.Direction == SapCooperSendInput.SapCooperSendDirection.SendOut)
                    {
                        result = await _cooperateAppService.SapCooperSendOut(cooperInput);
                    }
                    //else if (cooperInput.Direction == SapCooperSendInput.SapCooperSendDirection.SendBack)
                    //{
                    //    result = await _cooperateAppService.SapCooperSendBack(cooperInput);
                    //}

                    if (result)
                        receivedBillLineIds.Add(param.LineId);
                }
                catch (Exception ex)
                {
                    //确保其中任意一个交接行执行失败时，不想影响后续交接行的继续执行
                    Logger.Error("交接单接收失败！", ex);
                }
            }

            //更新接收成功行状态
            using (var uow = this.UnitOfWorkManager.Begin())
            {
                var bill = await this._handOverBillRepository.GetAll()
                   .Include(b => b.BillLines)
                   .FirstAsync(b => b.Id == billId);

                var billLines = bill.BillLines.Where(l => receivedBillLineIds.Contains(l.Id)).ToList();
                foreach (var line in billLines)
                {
                    //外协成功，更新交接单行状态
                    line.LineState = HandOverBillLineState.Received;
                    line.OperatorUserId = currentUser.Id;
                    line.OperatorUserName = currentUser.Surname;
                    line.OperatorDate = DateTime.Now;
                    await this._handOverBillLineRepository.UpdateAsync(line);
                }

                await this.CurrentUnitOfWork.SaveChangesAsync();    //提交一次UOW，刷新bill数据
                bool isAllComplete = await this.checkBillCompleted(bill);
                uow.Complete();
                return isAllComplete;
            }
        }

        /// <summary>
        /// 退回选中交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        /// <returns>交接单是否全部完成</returns>
        public async Task<bool> RejectHandOverBillLines(int billId, int[] lineIds)
        {
            var currentUser = await this.GetCurrentUserAsync();

            var bill = await this._handOverBillRepository.GetAll()
                .Include(b => b.BillLines)
                .FirstAsync(b => b.Id == billId);

            if (bill.BillState != HandOverBillState.Published)
                throw new UserFriendlyException($"交接单[{billId}]状态为[{bill.BillState.GetDescription()}]，不可执行退回操作！");

            var billLines = bill.BillLines.Where(l => lineIds.Contains(l.Id)).ToList();
            foreach (var line in billLines)
            {
                if (line.LineState != HandOverBillLineState.Pending)
                    continue;

                line.LineState = HandOverBillLineState.Rejected;
                line.OperatorUserId = currentUser.Id;
                line.OperatorUserName = currentUser.Surname;
                line.OperatorDate = DateTime.Now;
                await this._handOverBillLineRepository.UpdateAsync(line);
            }

            await this.CurrentUnitOfWork.SaveChangesAsync();    //提交一次UOW，刷新bill数据
            return await this.checkBillCompleted(bill);
        }

        //检查并交接单是否完成
        private async Task<bool> checkBillCompleted(HandOverBill bill)
        {
            var allCompleted = bill.BillLines.All(l => l.LineState != HandOverBillLineState.Pending);
            if (allCompleted)
            {
                bill.BillState = HandOverBillState.Completed;
                await this._handOverBillRepository.UpdateAsync(bill);
                return true;
            }
            return false;
        }

        #endregion
    }
}
