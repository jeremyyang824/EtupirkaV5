using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal.MultiTenancy.Dto;

namespace Etupirka.Application.Portal.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        /// <summary>
        /// 取得指定租户
        /// </summary>
        /// <param name="id">租户ID</param>
        Task<TenantOutput> GetTenant(int id);

        /// <summary>
        /// 获取有效的[租户编码,租户名]
        /// </summary>
        Task<List<NameValueDto>> GetActiveTenancyNames();

        /// <summary>
        /// 获取租户分页列表
        /// </summary>
        Task<PagedResultDto<TenantOutput>> GetTenants(GetTenantsInput input);

        /// <summary>
        /// 创建租户
        /// </summary>
        Task CreateTenant(CreateTenantInput input);

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id">租户ID</param>
        Task DeleteTenant(int id);

        /// <summary>
        /// 更新租户信息
        /// </summary>
        /// <param name="input">租户更新信息</param>
        Task UpdateTenant(EditTenantInput input);

        /// <summary>
        /// 改变租户启用状态
        /// </summary>
        /// <param name="id">租户ID</param>
        /// <param name="isActive">是否启用</param>
        Task ToggleTenantState(int id, bool isActive);
    }
}
