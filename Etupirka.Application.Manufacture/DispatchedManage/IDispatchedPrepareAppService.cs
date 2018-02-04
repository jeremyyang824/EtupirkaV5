using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage
{
    public interface IDispatchedPrepareAppService : IApplicationService
    {
        Task<DispatchedWorkerOutput> DoWorkForDispatched(DispatchedWorkerInput input);

        Task<StepResultOutput> FinishDispatchPrepareStatus_Tooling(SetPrepareStatusInput input);

        Task<StepResultOutput> FinishDispatchPrepareStatus_NC(SetPrepareStatusInput input);

        Task<IPagedResult<PrepareInfoWithStatusOutput>> FindPrepareInfos_TL(FindPrepareInfosInput input);

        Task<IPagedResult<PrepareInfoWithStatusOutput>> FindPrepareInfos_NC(FindPrepareInfosInput input);
    }
}
