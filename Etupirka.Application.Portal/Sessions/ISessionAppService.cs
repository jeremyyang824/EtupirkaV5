using System.Threading.Tasks;
using Abp.Application.Services;
using Etupirka.Application.Portal.Sessions.Dto;

namespace Etupirka.Application.Portal.Sessions
{
    /// <summary>
    /// 当前Session相关信息
    /// </summary>
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
