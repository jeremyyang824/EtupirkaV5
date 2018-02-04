namespace Etupirka.EntityFramework.Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abp113 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpUserClaims", newName: "t_sys_UserClaims");
            AlterColumn("dbo.t_sys_Users", "EmailConfirmationCode", c => c.String(maxLength: 328));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_sys_Users", "EmailConfirmationCode", c => c.String(maxLength: 128));
            RenameTable(name: "dbo.t_sys_UserClaims", newName: "AbpUserClaims");
        }
    }
}
