using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Etupirka.Domain.Portal.Authorization;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.Configuration;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Notifications;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class EtupirkaPortalDomainModule : AbpModule
    {
        public override void PreInitialize()
        {
            //匿名用户是否记录审计日志
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //声明权限相关实体类型
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(SysTenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(SysRole);
            Configuration.Modules.Zero().EntityTypes.User = typeof(SysUser);

            //！！！该系统是否启用多租户
            Configuration.MultiTenancy.IsEnabled = false;

            //本地化数据源（添加语言包）
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    EtupirkaPortalConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Etupirka.Domain.Portal.Localization.Source"
                        )
                    )
                );

            //Adding feature providers
            //Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<EtupirkaPortalNotificationProvider>();

            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //角色配置(添加静态角色)
            AppSysRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
