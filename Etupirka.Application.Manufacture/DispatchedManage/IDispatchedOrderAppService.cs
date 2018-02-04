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
    /// <summary>
    /// 机台作业映射管理
    /// </summary>
    public interface IDispatchedOrderAppService : IApplicationService
    {
        Task<PagedResultDto<DispatchedOrderOutput>> FindDispatchOrderPagerByWorkCenter(FindOrdersInput input);

       
    }
}
