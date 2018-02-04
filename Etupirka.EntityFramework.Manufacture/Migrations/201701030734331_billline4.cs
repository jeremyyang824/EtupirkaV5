namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billline4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseId", c => c.String());
            AddColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseName", c => c.String());
            AddColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseId", c => c.String());
            AddColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseName");
            DropColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseId");
            DropColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseName");
            DropColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseId");
        }
    }
}
