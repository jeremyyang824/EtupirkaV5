using Abp.Application.Navigation;
using Abp.Localization;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.Authorization;

namespace Etupirka.Web
{
    public class EtupirkaNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            /*
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",
                        L("HomePage"),
                        url: "#/",
                        icon: "fa fa-home")
                )
                .AddItem(
                    new MenuItemDefinition(
                        "SapOrder",
                        L("SAP订单"),
                        url: null,
                        icon: "fa fa-home")
                            .AddItem(
                                new MenuItemDefinition(
                                    "SapOrder_ListProcess",
                                    L("工艺列表"),
                                    url: "#handOverBills",
                                    icon: "fa fa-leaf")
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "HandOver",
                        L("交接管理"),
                        url: null,
                        icon: "fa fa-home")
                            .AddItem(
                                new MenuItemDefinition(
                                    "HandOver_ListHandOverBills",
                                    L("交接单列表"),
                                    url: "#handOverBills",
                                    icon: "fa fa-leaf")
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "SystemConfig",
                        L("系统配置"),
                        url: null,
                        icon: "fa fa-cog",
                        requiredPermissionName: AppPermissions.SystemConfig)
                            .AddItem(
                                new MenuItemDefinition(
                                    "SystemConfig_ListTenants",
                                    L("烟厂列表"),
                                    url: "#tenants",
                                    icon: "fa fa-leaf",
                                    requiredPermissionName: AppPermissions.SystemConfig_ListTenants)
                            )
                            .AddItem(
                                new MenuItemDefinition(
                                    "SystemConfig_ListUsers",
                                    L("用户管理"),
                                    url: "#users",
                                    icon: "fa fa-user",
                                    requiredPermissionName: AppPermissions.SystemConfig_ListUsers)
                            )
                            .AddItem(
                                new MenuItemDefinition(
                                    "SystemConfig_ListRoles",
                                    L("角色管理"),
                                    url: "#roles",
                                    icon: "fa fa-group",
                                    requiredPermissionName: AppPermissions.SystemConfig_ListRoles)
                            )
                );
            */
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EtupirkaPortalConsts.LocalizationSourceName);
        }
    }
}