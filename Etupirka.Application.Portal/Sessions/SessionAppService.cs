using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Etupirka.Application.Portal.Sessions.Dto;
using System.Linq;

namespace Etupirka.Application.Portal.Sessions
{
    /// <summary>
    /// 会话管理
    /// </summary>
    [AbpAuthorize]
    public class SessionAppService : EtupirkaAppServiceBase, ISessionAppService
    {
        /// <summary>
        /// 取得当前会话信息（用户、租户信息）
        /// </summary>
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var currentUser = await GetCurrentUserAsync();
            var organizationUnit = (await this.UserManager.GetOrganizationUnitsAsync(currentUser))?.FirstOrDefault();
            var output = new GetCurrentLoginInformationsOutput
            {
                User = currentUser.MapTo<UserLoginInfoDto>(),
                OrganizationUnit = organizationUnit.MapTo<UserOrganizationUnitInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}