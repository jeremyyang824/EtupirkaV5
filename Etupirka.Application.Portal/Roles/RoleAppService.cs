using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Etupirka.Application.Portal.Roles.Dto;
using Etupirka.Domain.Portal.Authorization.Roles;

namespace Etupirka.Application.Portal.Roles
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleAppService : EtupirkaAppServiceBase, IRoleAppService
    {
        private readonly SysRoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(SysRoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        /// <param name="id">角色ID</param>
        public async Task<RoleOutput> GetRole(int id)
        {
            var role = await this._roleManager.GetRoleByIdAsync(id);
            return role.MapTo<RoleOutput>();
        }

        /// <summary>
        /// 取得角色的权限
        /// </summary>
        /// <param name="id">角色ID</param>
        public async Task<string[]> GetRolePermissions(int id)
        {
            var role = await this._roleManager.GetRoleByIdAsync(id);
            var permissions = await _roleManager.GetGrantedPermissionsAsync(role);
            return permissions.Select(p => p.Name).ToArray();
        }

        /// <summary>
        /// 取得所有角色
        /// </summary>
        public async Task<ListResultDto<RoleOutput>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new ListResultDto<RoleOutput>(roles.MapTo<List<RoleOutput>>());
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        /// <summary>
        /// 创建新角色
        /// </summary>
        public async Task CreateRole(CreateRoleInput input)
        {
            var role = new SysRole(AbpSession.TenantId, input.Name, input.DisplayName) { IsDefault = input.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        public async Task UpdateRole(EditRoleInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            role.Name = input.Name;
            role.DisplayName = input.DisplayName;
            role.IsDefault = input.IsDefault;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task DeleteRole(int id)
        {
            var role = await _roleManager.GetRoleByIdAsync(id);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }
    }
}