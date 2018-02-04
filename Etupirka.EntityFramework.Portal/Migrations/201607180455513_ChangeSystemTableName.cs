namespace Etupirka.EntityFramework.Portal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSystemTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpAuditLogs", newName: "t_sys_AuditLogs");
            RenameTable(name: "dbo.AbpBackgroundJobs", newName: "t_sys_BackgroundJobs");
            RenameTable(name: "dbo.AbpFeatures", newName: "t_sys_Features");
            RenameTable(name: "dbo.AbpEditions", newName: "t_sys_Editions");
            RenameTable(name: "dbo.AbpLanguages", newName: "t_sys_Languages");
            RenameTable(name: "dbo.AbpLanguageTexts", newName: "t_sys_LanguageTexts");
            RenameTable(name: "dbo.AbpNotifications", newName: "t_sys_Notifications");
            RenameTable(name: "dbo.AbpNotificationSubscriptions", newName: "t_sys_NotificationSubscriptions");
            RenameTable(name: "dbo.AbpOrganizationUnits", newName: "t_sys_OrganizationUnits");
            RenameTable(name: "dbo.AbpPermissions", newName: "t_sys_Permissions");
            RenameTable(name: "dbo.AbpRoles", newName: "t_sys_Roles");
            RenameTable(name: "dbo.AbpUsers", newName: "t_sys_Users");
            RenameTable(name: "dbo.AbpUserLogins", newName: "t_sys_UserLogins");
            RenameTable(name: "dbo.AbpUserRoles", newName: "t_sys_UserRoles");
            RenameTable(name: "dbo.AbpSettings", newName: "t_sys_Settings");
            RenameTable(name: "dbo.AbpTenantNotifications", newName: "t_sys_TenantNotifications");
            RenameTable(name: "dbo.AbpTenants", newName: "t_sys_Tenants");
            RenameTable(name: "dbo.AbpUserAccounts", newName: "t_sys_UserAccounts");
            RenameTable(name: "dbo.AbpUserLoginAttempts", newName: "t_sys_UserLoginAttempts");
            RenameTable(name: "dbo.AbpUserNotifications", newName: "t_sys_UserNotifications");
            RenameTable(name: "dbo.AbpUserOrganizationUnits", newName: "t_sys_UserOrganizationUnits");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.t_sys_UserOrganizationUnits", newName: "AbpUserOrganizationUnits");
            RenameTable(name: "dbo.t_sys_UserNotifications", newName: "AbpUserNotifications");
            RenameTable(name: "dbo.t_sys_UserLoginAttempts", newName: "AbpUserLoginAttempts");
            RenameTable(name: "dbo.t_sys_UserAccounts", newName: "AbpUserAccounts");
            RenameTable(name: "dbo.t_sys_Tenants", newName: "AbpTenants");
            RenameTable(name: "dbo.t_sys_TenantNotifications", newName: "AbpTenantNotifications");
            RenameTable(name: "dbo.t_sys_Settings", newName: "AbpSettings");
            RenameTable(name: "dbo.t_sys_UserRoles", newName: "AbpUserRoles");
            RenameTable(name: "dbo.t_sys_UserLogins", newName: "AbpUserLogins");
            RenameTable(name: "dbo.t_sys_Users", newName: "AbpUsers");
            RenameTable(name: "dbo.t_sys_Roles", newName: "AbpRoles");
            RenameTable(name: "dbo.t_sys_Permissions", newName: "AbpPermissions");
            RenameTable(name: "dbo.t_sys_OrganizationUnits", newName: "AbpOrganizationUnits");
            RenameTable(name: "dbo.t_sys_NotificationSubscriptions", newName: "AbpNotificationSubscriptions");
            RenameTable(name: "dbo.t_sys_Notifications", newName: "AbpNotifications");
            RenameTable(name: "dbo.t_sys_LanguageTexts", newName: "AbpLanguageTexts");
            RenameTable(name: "dbo.t_sys_Languages", newName: "AbpLanguages");
            RenameTable(name: "dbo.t_sys_Editions", newName: "AbpEditions");
            RenameTable(name: "dbo.t_sys_Features", newName: "AbpFeatures");
            RenameTable(name: "dbo.t_sys_BackgroundJobs", newName: "AbpBackgroundJobs");
            RenameTable(name: "dbo.t_sys_AuditLogs", newName: "AbpAuditLogs");
        }
    }
}
