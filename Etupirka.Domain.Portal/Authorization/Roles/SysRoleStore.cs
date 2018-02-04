using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Authorization.Roles
{
    public class SysRoleStore : AbpRoleStore<SysRole, SysUser>
    {
        public SysRoleStore(
            IRepository<SysRole> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                roleRepository,
                userRoleRepository,
                rolePermissionSettingRepository)
        { }
    }
}