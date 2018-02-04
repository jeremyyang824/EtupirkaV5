using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using System.Threading.Tasks;
using Etupirka.Domain.External.Entities.Dmes;

namespace Etupirka.Application.Manufacture.DispatchedManage
{
    /// <summary>
    /// 工作中心映射管理
    /// </summary>
    public interface IWorkCenterAppService : IApplicationService
    {
        Task<DmesWorkCenterOutput> GetWorkCenter(int id);

        /// <summary>
        ///  查询工作中心列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IPagedResult<DmesWorkCenterOutput>> FindWorkCenterList(FindWorkCentersInput input);
    }
}
