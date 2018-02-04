using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Authorization.Roles
{
    /// <summary>
    /// 系统角色管理
    /// </summary>
    public class SysRoleManager : AbpRoleManager<SysRole, SysUser>
    {
        public SysRoleManager(
           SysRoleStore store,
           IPermissionManager permissionManager,
           IRoleManagementConfig roleManagementConfig,
           ICacheManager cacheManager,
           IUnitOfWorkManager unitOfWorkManager)
            : base(
                store,
                permissionManager,
                roleManagementConfig,
                cacheManager,
                unitOfWorkManager)
        { }
    }
}
