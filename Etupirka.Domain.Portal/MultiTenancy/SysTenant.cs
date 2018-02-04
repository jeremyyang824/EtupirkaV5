using Abp.MultiTenancy;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.MultiTenancy
{
    /// <summary>
    /// 系统租户
    /// </summary>
    public class SysTenant : AbpTenant<SysUser>
    {
        public new const string TenancyNameRegex = "^[a-zA-Z0-9_-]{1,}$";   //重写租户编码规则

        public SysTenant()
        { }

        public SysTenant(string tenancyName, string name)
            : base(tenancyName, name)
        { }
    }
}
