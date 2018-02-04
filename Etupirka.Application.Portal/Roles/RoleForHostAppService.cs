using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using Etupirka.Application.Portal.Roles.Dto;

namespace Etupirka.Application.Portal.Roles
{
    /// <summary>
    /// Host管理Teanant角色
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public class RoleForHostAppService : EtupirkaAppServiceBase, IRoleForHostAppService
    {
        private readonly IRoleAppService _roleAppService;

        public RoleForHostAppService(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        public async Task<RoleOutput> GetRole(int id, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return await _roleAppService.GetRole(id);
            }
        }

        /// <summary>
        /// 取得角色的权限
        /// </summary>
        public async Task<string[]> GetRolePermissions(int id, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return await _roleAppService.GetRolePermissions(id);
            }
        }

        /// <summary>
        /// 取得所有角色
        /// </summary>
        public async Task<ListResultDto<RoleOutput>> GetRoles(int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return await _roleAppService.GetRoles();
            }
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _roleAppService.UpdateRolePermissions(input);
            }
        }

        /// <summary>
        /// 创建新角色
        /// </summary>
        public async Task CreateRole(CreateRoleInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _roleAppService.CreateRole(input);
            }
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        public async Task UpdateRole(EditRoleInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _roleAppService.UpdateRole(input);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task DeleteRole(int id, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _roleAppService.DeleteRole(id);
            }
        }

    }
}
