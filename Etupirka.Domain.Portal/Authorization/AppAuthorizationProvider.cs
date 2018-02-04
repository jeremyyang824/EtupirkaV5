using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Etupirka.Domain.Portal.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var root = context.GetPermissionOrNull(AppPermissions.Root) ?? context.CreatePermission(AppPermissions.Root, L("Root"));

            //仓库管理
            //Commmon
            var warehouseManage = root.CreateChildPermission(AppPermissions.WarehouseManage, L("WarehouseManage"));
            warehouseManage.CreateChildPermission(AppPermissions.WarehouseManage_ListWarehouses, L("WarehouseManage_ListWarehouses"));
            warehouseManage.CreateChildPermission(AppPermissions.WarehouseManage_ListInventories, L("WarehouseManage_ListInventories"));
            warehouseManage.CreateChildPermission(AppPermissions.WarehouseManage_ListInventoryBills, L("WarehouseManage_ListInventoryBills"));
            //Host
            warehouseManage.CreateChildPermission(AppPermissions.WarehouseManage_ListItems, L("WarehouseManage_ListItems"), multiTenancySides: MultiTenancySides.Host);

            //寄售(领料流程)管理
            //Tenancy
            var itemReceiveManage = root.CreateChildPermission(AppPermissions.ItemReceiveManage, L("ItemReceiveManage"), multiTenancySides: MultiTenancySides.Tenant);
            itemReceiveManage.CreateChildPermission(AppPermissions.ItemReceiveManage_ItemShop, L("ItemReceiveManage_ItemShop"), multiTenancySides: MultiTenancySides.Tenant);
            itemReceiveManage.CreateChildPermission(AppPermissions.ItemReceiveManage_Cart, L("ItemReceiveManage_Cart"), multiTenancySides: MultiTenancySides.Tenant);
            itemReceiveManage.CreateChildPermission(AppPermissions.ItemReceiveManage_ListReceiveBills, L("ItemReceiveManage_ListReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);
            itemReceiveManage.CreateChildPermission(AppPermissions.ItemReceiveManage_ListVerifyReceiveBills, L("ItemReceiveManage_ListVerifyReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);
            itemReceiveManage.CreateChildPermission(AppPermissions.ItemReceiveManage_ListIssueReceiveBills, L("ItemReceiveManage_ListIssueReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);

            //寄售结算
            //Host
            var accountManage = root.CreateChildPermission(AppPermissions.AccountManage, L("AccountManage"), multiTenancySides: MultiTenancySides.Host);
            accountManage.CreateChildPermission(AppPermissions.AccountManage_ListPendings, L("AccountManage_ListPendings"), multiTenancySides: MultiTenancySides.Host);
            accountManage.CreateChildPermission(AppPermissions.AccountManage_ListAccountBills, L("AccountManage_ListAccountBills"), multiTenancySides: MultiTenancySides.Host);

            //待办通知
            //Commmon
            var taskNotify = root.CreateChildPermission(AppPermissions.TaskNotify, L("TaskNotify"));
            taskNotify.CreateChildPermission(AppPermissions.TaskNotify_MyCartItems, L("TaskNotify_MyCartItems"), multiTenancySides: MultiTenancySides.Tenant);
            taskNotify.CreateChildPermission(AppPermissions.TaskNotify_ToVerifyReceiveBills, L("TaskNotify_ToVerifyReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);
            taskNotify.CreateChildPermission(AppPermissions.TaskNotify_MyVerifiedReceiveBills, L("TaskNotify_MyVerifiedReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);
            taskNotify.CreateChildPermission(AppPermissions.TaskNotify_ToIssueReceiveBills, L("TaskNotify_ToIssueReceiveBills"), multiTenancySides: MultiTenancySides.Tenant);

            //系统配置
            //Common
            var systemConfig = root.CreateChildPermission(AppPermissions.SystemConfig, L("SystemConfig"));
            systemConfig.CreateChildPermission(AppPermissions.SystemConfig_ListUsers, L("SystemConfig_ListUsers"));
            systemConfig.CreateChildPermission(AppPermissions.SystemConfig_ListRoles, L("SystemConfig_ListRoles"));
            //Host
            systemConfig.CreateChildPermission(AppPermissions.SystemConfig_ListTenants, L("SystemConfig_ListTenants"), multiTenancySides: MultiTenancySides.Host);

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EtupirkaPortalConsts.LocalizationSourceName);
        }
    }
}