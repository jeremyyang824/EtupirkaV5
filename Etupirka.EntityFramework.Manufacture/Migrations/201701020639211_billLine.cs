namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billLine : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.t_inv_HandOverBill", newName: "t_manu_HandOverBill");
            CreateTable(
                "dbo.t_manu_HandOverBillLine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HandOverBillId = c.Int(nullable: false),
                        OrderInfo_SourceName = c.String(nullable: false, maxLength: 20),
                        OrderInfo_OrderNumber = c.String(nullable: false, maxLength: 50),
                        OrderInfo_LineNumber = c.Int(),
                        ItemNumber = c.String(nullable: false, maxLength: 50),
                        DrawingNumber = c.String(nullable: false, maxLength: 50),
                        ItemDescription = c.String(nullable: false, maxLength: 50),
                        HandOverQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentProcess_ProcessNumber = c.String(nullable: false, maxLength: 20),
                        CurrentProcess_ProcessCode = c.String(nullable: false, maxLength: 50),
                        CurrentProcess_ProcessName = c.String(nullable: false, maxLength: 50),
                        NextProcess_ProcessNumber = c.String(nullable: false, maxLength: 20),
                        NextProcess_ProcessCode = c.String(nullable: false, maxLength: 50),
                        NextProcess_ProcessName = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(nullable: false, maxLength: 500),
                        LineState = c.Int(nullable: false),
                        OperatorUserId = c.Long(),
                        OperatorUserName = c.String(maxLength: 20),
                        OperatorDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_manu_HandOverBill", t => t.HandOverBillId, cascadeDelete: true)
                .Index(t => t.HandOverBillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_manu_HandOverBillLine", "HandOverBillId", "dbo.t_manu_HandOverBill");
            DropIndex("dbo.t_manu_HandOverBillLine", new[] { "HandOverBillId" });
            DropTable("dbo.t_manu_HandOverBillLine");
            RenameTable(name: "dbo.t_manu_HandOverBill", newName: "t_inv_HandOverBill");
        }
    }
}
