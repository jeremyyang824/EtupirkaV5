using Abp.Authorization;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Authorization
{
    public class PermissionChecker : PermissionChecker<SysTenant, SysRole, SysUser>
    {
        public PermissionChecker(SysUserManager userManager)
            : base(userManager)
        { }
    }
}
