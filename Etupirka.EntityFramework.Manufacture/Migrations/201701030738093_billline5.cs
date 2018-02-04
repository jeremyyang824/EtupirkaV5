namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billline5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseId", c => c.String(maxLength: 50));
            AlterColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseName", c => c.String(maxLength: 50));
            AlterColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseId", c => c.String(maxLength: 50));
            AlterColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseName", c => c.String());
            AlterColumn("dbo.t_manu_HandOverBillLine", "NextProcess_PointOfUseId", c => c.String());
            AlterColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseName", c => c.String());
            AlterColumn("dbo.t_manu_HandOverBillLine", "CurrentProcess_PointOfUseId", c => c.String());
        }
    }
}
