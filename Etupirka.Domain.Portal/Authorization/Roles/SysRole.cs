using Abp.Authorization.Roles;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Authorization.Roles
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class SysRole : AbpRole<SysUser>
    {
        public SysRole()
        { }

        public SysRole(int? tenantId, string displayName)
            : base(tenantId, displayName)
        { }

        public SysRole(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        { }
    }
}
