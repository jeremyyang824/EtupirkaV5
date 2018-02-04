using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal.Roles.Dto;

namespace Etupirka.Application.Portal.Roles
{
    public interface IRoleForHostAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        Task<RoleOutput> GetRole(int id, int tenantId);

        /// <summary>
        /// 取得角色的权限
        /// </summary>
        Task<string[]> GetRolePermissions(int id, int tenantId);

        /// <summary>
        /// 取得所有角色
        /// </summary>
        Task<ListResultDto<RoleOutput>> GetRoles(int tenantId);

        /// <summary>
        /// 更新角色权限
        /// </summary>
        Task UpdateRolePermissions(UpdateRolePermissionsInput input, int tenantId);

        /// <summary>
        /// 创建新角色
        /// </summary>
        Task CreateRole(CreateRoleInput input, int tenantId);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        Task UpdateRole(EditRoleInput input, int tenantId);

        /// <summary>
        /// 删除角色
        /// </summary>
        Task DeleteRole(int id, int tenantId);
    }
}
