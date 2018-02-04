namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billLineInspect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_HandOverBillLine", "InspectState", c => c.Int(nullable: false));
            AddColumn("dbo.t_manu_HandOverBillLine", "InspectStateErrorMessage", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_HandOverBillLine", "InspectStateErrorMessage");
            DropColumn("dbo.t_manu_HandOverBillLine", "InspectState");
        }
    }
}
