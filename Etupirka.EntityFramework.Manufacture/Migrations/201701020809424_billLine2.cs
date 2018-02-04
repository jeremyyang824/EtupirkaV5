namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billLine2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_HandOverBill", "TransferTargetType", c => c.Int(nullable: false));
            AlterColumn("dbo.t_manu_HandOverBill", "TransferSource_OrganizationUnitCode", c => c.String());
            AlterColumn("dbo.t_manu_HandOverBill", "TransferTargetDepartment_OrganizationUnitCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_HandOverBill", "TransferTargetDepartment_OrganizationUnitCode", c => c.Long());
            AlterColumn("dbo.t_manu_HandOverBill", "TransferSource_OrganizationUnitCode", c => c.Long());
            DropColumn("dbo.t_manu_HandOverBill", "TransferTargetType");
        }
    }
}
