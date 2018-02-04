namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HandOverBill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_inv_HandOverBill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillCodePrefix = c.String(nullable: false, maxLength: 20),
                        BillCodeSerialNumber = c.Int(nullable: false),
                        TransferSource_OrganizationUnitId = c.Long(),
                        TransferSource_OrganizationUnitCode = c.Long(),
                        TransferSource_OrganizationUnitName = c.String(maxLength: 50),
                        TransferTargetDepartment_OrganizationUnitId = c.Long(),
                        TransferTargetDepartment_OrganizationUnitCode = c.Long(),
                        TransferTargetDepartment_OrganizationUnitName = c.String(maxLength: 50),
                        TransferTargetSupplier_SupplierCode = c.String(maxLength: 20),
                        TransferTargetSupplier_SupplierName = c.String(maxLength: 50),
                        Remark = c.String(nullable: false, maxLength: 500),
                        CreatorUserName = c.String(nullable: false, maxLength: 20),
                        HandOverDate = c.DateTime(),
                        BillState = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.t_inv_HandOverBill");
        }
    }
}
