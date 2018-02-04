namespace Etupirka.EntityFramework.Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotfix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_sys_OrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 95));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_sys_OrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
