using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.External.Entities.Dmes;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.External.Wintool;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Etupirka.Application.Manufacture.DispatchedManage
{
    public class DispatchedPrepareAppService : EtupirkaAppServiceBase, IDispatchedPrepareAppService
    {
        private readonly IRepository<SapMOrderProcessDispatchPrepare, int> _sapMOrderProcessDispatchPrepareRepository;
        private readonly IRepository<SapMOrderProcessDispatchPrepareStep, int> _sapMOrderProcessDispatchPrepareStepRepository;
        private readonly IRepository<DMESDispatchedWorker, int> _dMESDispatchedWorkerRepository;

        public readonly IDMESWorkCenterRepository _dmesWorkCenterRepository;
        public readonly IDMESWorkTicketRepository _dmesWorkTicketRepository;
        public readonly IWintoolApiRepository _winToolRepository;


        public DispatchedPrepareAppService(IRepository<SapMOrderProcessDispatchPrepare, int> sapMOrderProcessDispatchPrepareRepository, IRepository<SapMOrderProcessDispatchPrepareStep, int> sapMOrderProcessDispatchPrepareStepRepository, IRepository<DMESDispatchedWorker, int> dMESDispatchedWorkerRepository,
        IDMESWorkCenterRepository dmesWorkCenterRepository,
            IDMESWorkTicketRepository dmesWorkTicketRepository, IWintoolApiRepository winToolRepository)
        {
            this._sapMOrderProcessDispatchPrepareRepository = sapMOrderProcessDispatchPrepareRepository;
            this._sapMOrderProcessDispatchPrepareStepRepository = sapMOrderProcessDispatchPrepareStepRepository;
            this._dMESDispatchedWorkerRepository = dMESDispatchedWorkerRepository;
            this._dmesWorkCenterRepository = dmesWorkCenterRepository;
            this._dmesWorkTicketRepository = dmesWorkTicketRepository;
            this._winToolRepository = winToolRepository;
        }



        public async Task<DispatchedWorkerOutput> DoWorkForDispatched(DispatchedWorkerInput input)
        {
            //1、获取DMESDispatchedWorker最新数据
            //2、根据最新的同步完成时间，去获取（最新的同步完成时间-当前）的所有realeased派工单
            //3、根据获取的派工单，获取该派工单下所有与工单对应的唯一编号和工单的物料编码
            //3、以唯一编号为主键（派工单和工单1对1关系表的那个主键），进行本地齐备性流程状态创建
            //4、如果唯一编号已经有状态信息，不重复创建

            var workerlog = new DMESDispatchedWorker()
            {
                WorkerType = input.WorkerType,
                WorkerStartDate = DateTime.Now,
                WorkerResultMessage = ""
            };


            try
            {
                int workId = this._dMESDispatchedWorkerRepository.InsertAndGetId(workerlog);
                workerlog.Id = workId;

                var latestWorker = await this._dMESDispatchedWorkerRepository.GetAll()
                    .Where(b => b.IsWorkerSuccess)
                    .OrderByDescending(b => b.CreationTime)
                    .FirstOrDefaultAsync();

                List<AsyncResult> totalResults = new List<AsyncResult>();
                if (latestWorker != null && latestWorker.Id > 0)
                {
                    var newIds = await this._dmesWorkTicketRepository.GetDispatchedWorkTicketIDsLatest(
                        new DmesGetDispatchedIdsInput()
                        {
                            lastWorkerDate = latestWorker.WorkerStartDate
                        });

                    //Task < IList < DmesWorkcenterMapOutput >> 
                    var mappers = await _dmesWorkCenterRepository.GetWorkCenterWinToolMaps();

                    foreach (var idGet in newIds.Distinct())
                    {
                        AsyncResult asyncResult = new AsyncResult();
                        asyncResult.DispatchedWorkTicketId = idGet.DispatchWorKTicketID.ToString();
                        var existId = await this._sapMOrderProcessDispatchPrepareRepository.GetAll()
                             .Where(p => p.DispatchWorKTicketID == idGet.DispatchWorKTicketID)
                             .FirstOrDefaultAsync();

                        if (existId == null || existId.Id <= 0)
                        {
                            //新建
                            var newPrepareInfo = new SapMOrderProcessDispatchPrepare()
                            {
                                DispatchWorKTicketID = idGet.DispatchWorKTicketID
                            };
                            int newid = await this._sapMOrderProcessDispatchPrepareRepository.InsertAndGetIdAsync(newPrepareInfo);

                            existId = newPrepareInfo;
                        }

                        //MES下达新派工单时，发起流程：
                        //1) 刀具配刀流程
                        //2) NC程序准备流程                           

                        StepResultOutput resultTL = StepResultOutput.Failure;
                        if (!existId.Tooling_IsPreparedFinished.HasValue || existId.Tooling_IsPreparedFinished <= (short)SapMOrderProcessDispatchPrepareStepStatus.未准备)
                        {
                            resultTL = await createJob_Tooling(existId, idGet, mappers);
                            asyncResult.Tooling_IsSuccess = resultTL.IsSuccess;
                            asyncResult.Tooling_ResultMessage = resultTL.ResultMessage;
                        }
                        else
                        {
                            asyncResult.Tooling_IsSuccess = true;
                            asyncResult.Tooling_ResultMessage = "已存在";
                        }

                        StepResultOutput resultNC = StepResultOutput.Failure;
                        if (!existId.NC_IsPreparedFinished.HasValue || existId.NC_IsPreparedFinished <= (short)SapMOrderProcessDispatchPrepareStepStatus.未准备)
                        {
                            resultNC = await createJob_NC(existId, idGet, mappers);
                            asyncResult.NC_IsSuccess = resultNC.IsSuccess;
                            asyncResult.NC_ResultMessage = resultNC.ResultMessage;
                        }
                        else
                        {
                            asyncResult.NC_IsSuccess = true;
                            asyncResult.NC_ResultMessage = "已存在";
                        }

                        totalResults.Add(asyncResult);
                    }
                }
                int failcount = totalResults.Count(t => !(t.NC_IsSuccess && t.Tooling_IsSuccess));
                //var totalResultsStr = totalResults.Select(buildAsyncResult);
                var totalResultsStr = buildAsyncResult(totalResults);
                workerlog.WorkerFinishDate = DateTime.Now;
                workerlog.IsWorkerSuccess = failcount > 0 ? false : true;
                workerlog.WorkerResultMessage = totalResultsStr;

                await this._dMESDispatchedWorkerRepository.InsertOrUpdateAsync(workerlog);
                return await Task.FromResult(workerlog.MapTo<DispatchedWorkerOutput>());
            }
            catch (Exception e)
            {
                workerlog.WorkerFinishDate = DateTime.Now;
                workerlog.IsWorkerSuccess = false;
                workerlog.WorkerResultMessage = e.Message;

                await this._dMESDispatchedWorkerRepository.InsertOrUpdateAsync(workerlog);
                return await Task.FromResult(workerlog.MapTo<DispatchedWorkerOutput>());
            }
        }

        private string buildAsyncResult(List<AsyncResult> results)
        {
            List<string> builderSuccess = new List<string>();
            List<string> builderFailedTL = new List<string>();
            List<string> builderFailedNC = new List<string>();

            //builder += "DispatchedWorkTicketId:" + result.DispatchedWorkTicketId + ";;";
            //builder += "CreateJob:";
            //builder += "【Tooling--" + result.Tooling_IsSuccess + "--" + result.Tooling_ResultMessage + "】";
            //builder += "【NC--" + result.NC_IsSuccess + "--" + result.NC_ResultMessage + "】;;";

            results.ForEach(r =>
            {
                bool isSuccess = false;
                if (!r.Tooling_IsSuccess)
                {
                    builderFailedTL.Add("【" + r.DispatchedWorkTicketId + " -" + r.Tooling_ResultMessage + "】");
                    isSuccess = true;
                }
                if (!r.NC_IsSuccess)
                {
                    builderFailedNC.Add("【" + r.DispatchedWorkTicketId + "-" + r.NC_ResultMessage + "】");
                    isSuccess = true;
                }

                if (!isSuccess)
                {
                    builderSuccess.Add(r.DispatchedWorkTicketId);
                }
            });

            string strSuccess = "Success:" + builderSuccess.Count + ":" + string.Join(",", builderSuccess);
            string strFailureTL = "FailureTL:" + builderFailedTL.Count + ":" + string.Join("", builderFailedTL);
            string strFailureNC = "FailureNC:" + builderFailedNC.Count + ":" + string.Join("", builderFailedNC);
            return string.Format("Total：{0}；{1}；{2}；{3}", results.Count, strSuccess, strFailureTL, strFailureNC);
        }


        private async Task<StepResultOutput> createJob_Tooling(SapMOrderProcessDispatchPrepare prepareInfo, DmesDispatchedIdOutput idGet, IList<DmesWorkcenterMapOutput> mappers)
        {
            StepResultOutput result = StepResultOutput.Failure;
            var machine = mappers.SingleOrDefault(m => m.WORKID == idGet.ActualWorkID);
            DateTime jobCreate = DateTime.Now;
            result = await tryDispatchPrepareStep(async () =>
            {
                WinToolResult tJob = await _winToolRepository.WintoolCreateToolingJob(new Domain.External.Entities.Winchill.CreateJobInput()
                {
                    JobType = SapMOrderProcessDispatchPrepareStepTransTypes.TL,
                    PrepareInfoId = prepareInfo.Id,
                    ItemNumber = idGet.MaterialNumber,
                    MachineType = (machine != null && machine.ID > 0 && string.IsNullOrEmpty(machine.WinTool)) ? machine.WinTool : ""

                });
                return await Task.FromResult(new StepResultOutput(tJob));
            },
SapMOrderProcessDispatchPrepareStepTransTypes.TL, SapMOrderProcessDispatchPrepareStepTransTypes.TL_Start, prepareInfo);

            prepareInfo.Tooling_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.未准备;
            if (result.IsSuccess)
            {
                prepareInfo.Tooling_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.准备中;
                prepareInfo.Tooling_RequiredDate = jobCreate.AddDays(1);
            }
            await _sapMOrderProcessDispatchPrepareRepository.UpdateAsync(prepareInfo);
            return result;
        }

        private async Task<StepResultOutput> createJob_NC(SapMOrderProcessDispatchPrepare prepareInfo, DmesDispatchedIdOutput idGet, IList<DmesWorkcenterMapOutput> mappers)
        {
            StepResultOutput result = StepResultOutput.Failure;
            var machine = mappers.SingleOrDefault(m => m.WORKID == idGet.ActualWorkID);
            DateTime jobCreate = DateTime.Now;
            result = await tryDispatchPrepareStep(async () =>
            {
                WinToolResult nJob = await _winToolRepository.WintoolCreateNCJob(new Domain.External.Entities.Winchill.CreateJobInput()
                {
                    JobType = "NC",
                    PrepareInfoId = prepareInfo.Id,
                    ItemNumber = idGet.MaterialNumber,
                    MachineType = (machine != null && machine.ID > 0) ? machine.WinTool : ""

                });

                return await Task.FromResult(new StepResultOutput(nJob));
            },
SapMOrderProcessDispatchPrepareStepTransTypes.NC, SapMOrderProcessDispatchPrepareStepTransTypes.NC_Start, prepareInfo);

            prepareInfo.NC_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.未准备;
            if (result.IsSuccess)
            {
                prepareInfo.NC_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.准备中;
                prepareInfo.NC_RequiredDate = jobCreate.AddDays(1);
            }
            await _sapMOrderProcessDispatchPrepareRepository.UpdateAsync(prepareInfo);

            return result;
        }

        /// <summary>
        /// WinTool调用反馈完成情况  
        ///     刀具
        /// </summary>
        /// <param name="taskId">机台任务ID 就是 派工单和工单的关系表主键</param>
        /// <returns></returns>
        public async Task<StepResultOutput> FinishDispatchPrepareStatus_Tooling(SetPrepareStatusInput input)
        {
            StepResultOutput result = StepResultOutput.Failure;
            if (input == null)
                return result;
            if (input.PrepareInfoId <= 0)
                return result;

            var existId = await this._sapMOrderProcessDispatchPrepareRepository.GetAll()
                            .Where(p => p.DispatchWorKTicketID == input.PrepareInfoId)
                            .FirstOrDefaultAsync();

            if (existId == null || existId.Id <= 0)
                return result;

            try
            {
                result = await tryDispatchPrepareStep(async () =>
                {
                    return await Task.FromResult(StepResultOutput.Success);
                },
            SapMOrderProcessDispatchPrepareStepTransTypes.TL, SapMOrderProcessDispatchPrepareStepTransTypes.TL_Finished, existId);

                if (result.IsSuccess)
                {
                    existId.Tooling_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.已完成;
                    await _sapMOrderProcessDispatchPrepareRepository.UpdateAsync(existId);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// WinTool调用反馈完成情况  
        ///     刀具
        /// </summary>
        /// <param name="taskId">机台任务ID 就是 派工单和工单的关系表主键</param>
        /// <returns></returns>
        public async Task<StepResultOutput> FinishDispatchPrepareStatus_NC(SetPrepareStatusInput input)
        {
            StepResultOutput result = StepResultOutput.Failure;
            if (input == null)
                return result;
            if (input.PrepareInfoId <= 0)
                return result;

            var existId = await this._sapMOrderProcessDispatchPrepareRepository.GetAll()
                            .Where(p => p.DispatchWorKTicketID == input.PrepareInfoId)
                            .FirstOrDefaultAsync();

            if (existId == null || existId.Id <= 0)
                return result;

            try
            {
                result = await tryDispatchPrepareStep(async () =>
                {
                    return await Task.FromResult(StepResultOutput.Success);
                },
            SapMOrderProcessDispatchPrepareStepTransTypes.NC, SapMOrderProcessDispatchPrepareStepTransTypes.NC_Finished, existId);

                if (result.IsSuccess)
                {
                    existId.NC_IsPreparedFinished = (short)SapMOrderProcessDispatchPrepareStepStatus.已完成;
                    await _sapMOrderProcessDispatchPrepareRepository.UpdateAsync(existId);
                }
            }
            catch { }
            return result;

        }

        public async Task<bool> FindPrepareInfos(FindPrepareInfosInput input)
        {
            var query = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
                .Include(s => s.PrepareInfo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType == SapMOrderProcessDispatchPrepareStepTransTypes.ALL, p => (p.PrepareInfo.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus) || p.PrepareInfo.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)))
                .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.TL), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)))
                 .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.NC), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)));

            //var query = this._sapMOrderProcessDispatchPrepareRepository.GetAll()
            //   .Include(s => s.PrepareSteps).Where(p => p.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus) || p.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus));

            //var list = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
            //    .Include(s => s.PrepareInfo)
            //    .WhereIf(!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType != "all", p => p.StepTransactionType == input.PrepareType.ToUpper())
            //    .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.TL), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)))
            //     .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.NC), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)));

            var count = await query.CountAsync();
            // var prepareSteps = await query.OrderBy(p => p.PrepareInfo.)
            //var prepareSteps = await query
            //    .OrderBy(p => p.)
            //    .ThenByDescending(b => b.HandOverBill.BillCodePrefix)
            //    .ThenByDescending(b => b.HandOverBill.BillCodeSerialNumber)
            //    .PageBy(input).ToListAsync();
            return await Task.FromResult(false);

        }

        public async Task<IPagedResult<PrepareInfoWithStatusOutput>> FindPrepareInfos_TL(FindPrepareInfosInput input)
        {
            short prepareStatus = Convert.ToInt16(input.PrepareStatus);
            var query = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
                .Include(s => s.PrepareInfo)
                .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.TL), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished.Value == prepareStatus));

            //var query = this._sapMOrderProcessDispatchPrepareRepository.GetAll()
            //   .Include(s => s.PrepareSteps).Where(p => p.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus) || p.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus));

            //var list = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
            //    .Include(s => s.PrepareInfo)
            //    .WhereIf(!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType != "all", p => p.StepTransactionType == input.PrepareType.ToUpper())
            //    .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.TL), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)))
            //     .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.NC), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished == Convert.ToInt16(input.PrepareStatus)));

            var count = await query.CountAsync();
            var prepareInfos = await query.OrderBy(p => p.PrepareInfo.Tooling_RequiredDate).PageBy(input).ToListAsync();


            var preparegroup = prepareInfos.GroupBy(p => new SapMOrderProcessDispatchPrepareKey
            {
                DispatchWorKTicketID = p.PrepareInfo.DispatchWorKTicketID,
                StepStatus = p.PrepareInfo.Tooling_IsPreparedFinished.HasValue ? p.PrepareInfo.Tooling_IsPreparedFinished : (short)0,
                StepType = p.StepTransactionType

            }).Select(p => new
            {
                PrepareInfoKey = p.Key,
                PrepareStartedValue = p.Where(ps => ps.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && ps.StepName == SapMOrderProcessDispatchPrepareStepTransTypes.TL_Start).OrderByDescending(ps => ps.CreationTime).FirstOrDefault(),
                PrepareFinishedValue = p.Where(ps => ps.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && ps.StepName == SapMOrderProcessDispatchPrepareStepTransTypes.TL_Finished).OrderByDescending(ps => ps.CreationTime).FirstOrDefault()
                //PrepareStepValue = p.Where(ps => ps.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && ps.IsStepSuccess).OrderByDescending(ps => ps.CreationTime).ToList()
            });

            //var ids = prepareInfos.Select(p => p.PrepareInfo.DispatchWorKTicketID.ToString()).Distinct().ToList();
            var ids = preparegroup.Select(p => p.PrepareInfoKey.DispatchWorKTicketID.ToString()).Distinct().ToList();
            var orders = await _dmesWorkTicketRepository.FindDispatchedOrdersByDispatchWorkID(new DmesFindDispatchedOrderByTicketInput()
            {
                DispatchWorkIDs = ids
            });

            List<PrepareInfoWithStatusOutput> result = new List<PrepareInfoWithStatusOutput>();

            foreach (var info in preparegroup)
            {
                DmesOrderOutput orderBean = orders.SingleOrDefault(o => o.DispatchWorKTicketID == info.PrepareInfoKey.DispatchWorKTicketID);

                DateTime stepStarted = (info.PrepareStartedValue == null || info.PrepareStartedValue.Id <= 0) ? default(DateTime) : info.PrepareStartedValue.CreationTime;
                DateTime stepFinished = (info.PrepareFinishedValue == null || info.PrepareFinishedValue.Id <= 0) ? default(DateTime) : info.PrepareFinishedValue.CreationTime;
                PrepareInfoWithStatusOutput Outbean = new PrepareInfoWithStatusOutput()
                {
                    OrderNumber = orderBean.OrderNumber,
                    RoutingNumber = orderBean.RoutingNumber,
                    MaterialNumber = orderBean.MaterialNumber,
                    MaterialDescription = orderBean.MaterialDescription,
                    ActualWorkID = orderBean.ActualWorkID,
                    ActualWorkName = orderBean.ActualWorkName,


                    DispatchWorKTicketID = info.PrepareInfoKey.DispatchWorKTicketID,
                    StepTransactionType = info.PrepareInfoKey.StepType,
                    StepStatus = info.PrepareInfoKey.StepStatus,
                    StepStatusStr = Enum.GetName(typeof(SapMOrderProcessDispatchPrepareStepStatus), info.PrepareInfoKey.StepStatus),

                    StepStartedDate = stepStarted == default(DateTime) ? "" : stepStarted.ToString("yyyy-MM-dd HH:mm:ss"),
                    StepRequiredDate = stepStarted == default(DateTime) ? "" : stepStarted.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),

                    StepFinishedDate = stepFinished == DateTime.MinValue ? "" : stepFinished.ToString("yyyy-MM-dd HH:mm:ss"),

                    StepDelayed = stepFinished > stepStarted.AddDays(1) ? "超期" : ((stepFinished == DateTime.MinValue && stepStarted.AddDays(1) <= DateTime.Now) ? "超期" : "")
                };

                result.Add(Outbean);
            }

            return new PagedResultDto<PrepareInfoWithStatusOutput>(count, result);

        }


        public async Task<IPagedResult<PrepareInfoWithStatusOutput>> FindPrepareInfos_NC(FindPrepareInfosInput input)
        {
            short prepareStatus = Convert.ToInt16(input.PrepareStatus);
            var query = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
               .Include(s => s.PrepareInfo)
               //.WhereIf(!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType == SapMOrderProcessDispatchPrepareStepTransTypes.ALL, p => ((p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished == prepareStatus)
               //|| (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished == prepareStatus)))
               .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.TL), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && p.PrepareInfo.Tooling_IsPreparedFinished == prepareStatus))
                .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.NC), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished == prepareStatus));
            //var query = this._sapMOrderProcessDispatchPrepareStepRepository.GetAll()
            //    .Include(s => s.PrepareInfo)
            //    .WhereIf((!string.IsNullOrWhiteSpace(input.PrepareType) && input.PrepareType.Trim().ToUpper() == SapMOrderProcessDispatchPrepareStepTransTypes.NC), p => (p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC && p.PrepareInfo.NC_IsPreparedFinished.Value == prepareStatus));


            var count = await query.CountAsync();
            var prepareInfos = await query
                .Select(p => new
                {
                    Id = p.Id,
                    DispatchWorKTicketID = p.PrepareInfo.DispatchWorKTicketID,
                    StepTransactionType = p.StepTransactionType,
                    StepName = p.StepName,
                    IsPreparedFinished = p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL ? p.PrepareInfo.Tooling_IsPreparedFinished : (
                    p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC ? p.PrepareInfo.NC_IsPreparedFinished : (short)0),
                    StepRequiredDate = p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL ? p.PrepareInfo.Tooling_RequiredDate : (
                    p.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.NC ? p.PrepareInfo.NC_RequiredDate : default(DateTime)),
                    CreationTime = p.CreationTime

                }).OrderBy(p => p.StepRequiredDate).PageBy(input).ToListAsync();


            var preparegroup = prepareInfos.GroupBy(p => new SapMOrderProcessDispatchPrepareKey
            {
                DispatchWorKTicketID = p.DispatchWorKTicketID,
                StepStatus = p.IsPreparedFinished.HasValue ? p.IsPreparedFinished : (short)0,
                StepType = p.StepTransactionType

            }).Select(p => new
            {
                PrepareInfoKey = p.Key,
                PrepareStartedValue = p.Where(ps => ps.StepName == ps.StepTransactionType + " Started").OrderByDescending(ps => ps.CreationTime).FirstOrDefault(),
                PrepareFinishedValue = p.Where(ps => ps.StepName == ps.StepTransactionType + " Finished").OrderByDescending(ps => ps.CreationTime).FirstOrDefault()
                //PrepareStepValue = p.Where(ps => ps.StepTransactionType == SapMOrderProcessDispatchPrepareStepTransTypes.TL && ps.IsStepSuccess).OrderByDescending(ps => ps.CreationTime).ToList()
            });

            //var ids = prepareInfos.Select(p => p.PrepareInfo.DispatchWorKTicketID.ToString()).Distinct().ToList();
            var ids = preparegroup.Select(p => p.PrepareInfoKey.DispatchWorKTicketID.ToString()).Distinct().ToList();
            var orders = await _dmesWorkTicketRepository.FindDispatchedOrdersByDispatchWorkID(new DmesFindDispatchedOrderByTicketInput()
            {
                DispatchWorkIDs = ids
            });

            List<PrepareInfoWithStatusOutput> result = new List<PrepareInfoWithStatusOutput>();

            foreach (var info in preparegroup)
            {
                DmesOrderOutput orderBean = orders.SingleOrDefault(o => o.DispatchWorKTicketID == info.PrepareInfoKey.DispatchWorKTicketID);

                DateTime stepStarted = (info.PrepareStartedValue == null || info.PrepareStartedValue.Id <= 0) ? default(DateTime) : info.PrepareStartedValue.CreationTime;
                DateTime stepFinished = (info.PrepareFinishedValue == null || info.PrepareFinishedValue.Id <= 0) ? default(DateTime) : info.PrepareFinishedValue.CreationTime;
                PrepareInfoWithStatusOutput Outbean = new PrepareInfoWithStatusOutput()
                {
                    OrderNumber = orderBean.OrderNumber,
                    RoutingNumber = orderBean.RoutingNumber,
                    MaterialNumber = orderBean.MaterialNumber,
                    MaterialDescription = orderBean.MaterialDescription,
                    ActualWorkID = orderBean.ActualWorkID,
                    ActualWorkName = orderBean.ActualWorkName,


                    DispatchWorKTicketID = info.PrepareInfoKey.DispatchWorKTicketID,
                    StepTransactionType = info.PrepareInfoKey.StepType,
                    StepStatus = info.PrepareInfoKey.StepStatus,
                    StepStatusStr = Enum.GetName(typeof(SapMOrderProcessDispatchPrepareStepStatus), info.PrepareInfoKey.StepStatus),

                    StepStartedDate = stepStarted == default(DateTime) ? "" : stepStarted.ToString("yyyy-MM-dd HH:mm:ss"),
                    StepRequiredDate = stepStarted == default(DateTime) ? "" : stepStarted.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),

                    StepFinishedDate = stepFinished == DateTime.MinValue ? "" : stepFinished.ToString("yyyy-MM-dd HH:mm:ss"),

                    StepDelayed = stepFinished > stepStarted.AddDays(1) ? "超期" : ((stepFinished == DateTime.MinValue && stepStarted.AddDays(1) <= DateTime.Now) ? "超期" : "")
                };

                result.Add(Outbean);
            }

            return new PagedResultDto<PrepareInfoWithStatusOutput>(count, result);

        }


        //执行一个过程方法，（无论成功与否）记录一条日志
        private async Task<StepResultOutput> tryDispatchPrepareStep(Func<Task<StepResultOutput>> tryFunc,
            string stepTransactionType, string stepName, SapMOrderProcessDispatchPrepare prepareInfo)
        {
            //执行该步骤
            StepResultOutput result = StepResultOutput.Failure;
            try
            {
                result = await tryFunc();
                return result;
            }
            catch (Exception ex)
            {
                result = new StepResultOutput(false, ex.Message);
                return result;
            }
            finally
            {
                var step = await _sapMOrderProcessDispatchPrepareStepRepository.FirstOrDefaultAsync(s => s.SapMOrderProcessDispatchPrepareId == prepareInfo.Id && s.StepTransactionType == stepTransactionType && s.StepName == stepName);
                if (step == null || step.Id <= 0)
                    step = new SapMOrderProcessDispatchPrepareStep() { };
                step.IsStepSuccess = result.IsSuccess;
                step.SapMOrderProcessDispatchPrepareId = prepareInfo.Id;
                step.StepTransactionType = stepTransactionType;
                step.StepName = stepName;
                step.StepResultMessage = result.ResultMessage;
                //StepStatus = (short)SapMOrderProcessDispatchPrepareStepStatus.准备中,                                     
                //步骤记录
                await _sapMOrderProcessDispatchPrepareStepRepository.InsertOrUpdateAsync(step);
            }
        }

        ///// <summary>
        ///// 步骤执行结果
        ///// </summary>
        //private class StepResultOutput
        //{
        //    /// <summary>
        //    /// 是否执行成功
        //    /// </summary>
        //    public bool IsSuccess { get; set; }
        //    /// <summary>
        //    /// 执行反馈消息
        //    /// </summary>
        //    public string ResultMessage { get; set; }

        //    public StepResultOutput(bool isSuccess, string message = "")
        //    {
        //        this.IsSuccess = isSuccess;
        //        this.ResultMessage = message;
        //    }

        //    public StepResultOutput(WinToolResult wintoolResult)
        //    {
        //        this.IsSuccess = wintoolResult.IsSuccess;
        //        this.ResultMessage = wintoolResult.Message;
        //    }


        //    public static readonly StepResultOutput Failure = new StepResultOutput(false);

        //    public static readonly StepResultOutput Success = new StepResultOutput(true);
        //}

        /// <summary>
        /// 步骤执行结果
        /// </summary>
        private class AsyncResult
        {
            public string DispatchedWorkTicketId { get; set; }

            /// <summary>
            /// 是否执行成功
            /// </summary>
            public bool Tooling_IsSuccess { get; set; }

            /// <summary>
            /// 执行反馈消息
            /// </summary>
            public string Tooling_ResultMessage { get; set; }

            /// <summary>
            /// 是否执行成功
            /// </summary>
            public bool NC_IsSuccess { get; set; }


            /// <summary>
            /// 执行反馈消息
            /// </summary>
            public string NC_ResultMessage { get; set; }

            public AsyncResult(string dispatchedWorkTicketId = "0", bool tooling_IsSuccess = false, bool nc_IsSuccess = false, string tooling_ResultMessage = "", string nc_ResultMessage = "")
            {
                this.Tooling_IsSuccess = tooling_IsSuccess;
                this.NC_IsSuccess = nc_IsSuccess;
                this.Tooling_ResultMessage = tooling_ResultMessage;
                this.NC_ResultMessage = nc_ResultMessage;
                this.DispatchedWorkTicketId = dispatchedWorkTicketId;
            }

        }

    }
}
