using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Zero;
using Etupirka.Domain.Portal.Editions;
using Etupirka.Domain.Portal.Users;
using Microsoft.AspNet.Identity;

namespace Etupirka.Domain.Portal.MultiTenancy
{
    /// <summary>
    /// 系统租户管理
    /// </summary>
    public class SysTenantManager : AbpTenantManager<SysTenant, SysUser>
    {
        public SysTenantManager(
            IRepository<SysTenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore)
            : base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore)
        { }

        /// <summary>
        /// 重写租户编码验证规则
        /// </summary>
        protected override async Task<IdentityResult> ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, SysTenant.TenancyNameRegex))
            {
                return AbpIdentityResult.Failed(L("InvalidTenancyName"));
            }

            return IdentityResult.Success;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }
    }
}