namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessCooperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_manu_SapMOrderProcessCooperate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SapMOrderProcessId = c.Guid(nullable: false),
                        CooperateType = c.Int(nullable: false),
                        CooperaterCode = c.String(nullable: false, maxLength: 50),
                        CooperaterName = c.String(nullable: false, maxLength: 200),
                        CooperaterPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsSapPomtFinished = c.Boolean(),
                        SapPoNumber = c.String(maxLength: 50),
                        IsFsComtFinished = c.Boolean(),
                        FsCoNumber = c.String(maxLength: 50),
                        IsFsMomtFinished = c.Boolean(),
                        FsMoNumber = c.String(maxLength: 50),
                        IsFsPickFinished = c.Boolean(),
                        IsFsMorvFinished = c.Boolean(),
                        IsFsImtrFinished = c.Boolean(),
                        IsFsImtrSalesFinished = c.Boolean(),
                        IsFsShipFinished = c.Boolean(),
                        IsSapPorvFinished = c.Boolean(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_manu_SapMOrderProcess", t => t.SapMOrderProcessId)
                .Index(t => t.SapMOrderProcessId);
            
            CreateTable(
                "dbo.t_manu_SapMOrderProcessCooperateStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SapMOrderProcessCooperateId = c.Int(nullable: false),
                        StepTransactionType = c.String(nullable: false, maxLength: 50),
                        StepName = c.String(nullable: false, maxLength: 100),
                        IsStepSuccess = c.Boolean(nullable: false),
                        StepResultMessage = c.String(nullable: false, maxLength: 2000),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_manu_SapMOrderProcessCooperate", t => t.SapMOrderProcessCooperateId, cascadeDelete: true)
                .Index(t => t.SapMOrderProcessCooperateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_manu_SapMOrderProcessCooperate", "SapMOrderProcessId", "dbo.t_manu_SapMOrderProcess");
            DropForeignKey("dbo.t_manu_SapMOrderProcessCooperateStep", "SapMOrderProcessCooperateId", "dbo.t_manu_SapMOrderProcessCooperate");
            DropIndex("dbo.t_manu_SapMOrderProcessCooperateStep", new[] { "SapMOrderProcessCooperateId" });
            DropIndex("dbo.t_manu_SapMOrderProcessCooperate", new[] { "SapMOrderProcessId" });
            DropTable("dbo.t_manu_SapMOrderProcessCooperateStep");
            DropTable("dbo.t_manu_SapMOrderProcessCooperate");
        }
    }
}
