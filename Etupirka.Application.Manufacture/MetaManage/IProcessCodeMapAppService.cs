using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.MetaManage.Dto;

namespace Etupirka.Application.Manufacture.MetaManage
{
    /// <summary>
    /// SAP/FS����ӳ�����
    /// </summary>
    public interface IProcessCodeMapAppService : IApplicationService
    {
        /// <summary>
        /// ȡ�����й���ӳ��
        /// </summary>
        Task<ListResultDto<ProcessCodeMapOutput>> GetAllProcessCodeMap();
    }
}