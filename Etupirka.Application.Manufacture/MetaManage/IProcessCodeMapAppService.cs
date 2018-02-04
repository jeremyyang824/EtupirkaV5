using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.MetaManage.Dto;

namespace Etupirka.Application.Manufacture.MetaManage
{
    /// <summary>
    /// SAP/FS工艺映射管理
    /// </summary>
    public interface IProcessCodeMapAppService : IApplicationService
    {
        /// <summary>
        /// 取得所有工艺映射
        /// </summary>
        Task<ListResultDto<ProcessCodeMapOutput>> GetAllProcessCodeMap();
    }
}