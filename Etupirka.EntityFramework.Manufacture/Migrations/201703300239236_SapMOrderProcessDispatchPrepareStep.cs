namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapMOrderProcessDispatchPrepareStep : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_manu_SapMOrderProcessDispatchPrepare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DispatchTicketID = c.String(nullable: false),
                        WorkCenterID = c.String(),
                        NC_IsPreparedFinished = c.Boolean(),
                        Tooling_IsPreparedFinished = c.Boolean(),
                        Mould_IsPreparedFinished = c.Boolean(),
                        Special_IsPreparedFinished = c.Boolean(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_manu_SapMOrderProcessDispatchPrepareStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SapMOrderProcessDispatchPrepareId = c.Int(nullable: false),
                        StepTransactionType = c.String(nullable: false, maxLength: 50),
                        StepName = c.String(nullable: false, maxLength: 100),
                        IsStepSuccess = c.Boolean(nullable: false),
                        StepResultMessage = c.String(nullable: false, maxLength: 2000),
                        StepStartDate = c.DateTime(),
                        StepRequiredDate = c.DateTime(),
                        StepFinishDate = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_manu_SapMOrderProcessDispatchPrepare", t => t.SapMOrderProcessDispatchPrepareId, cascadeDelete: true)
                .Index(t => t.SapMOrderProcessDispatchPrepareId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "SapMOrderProcessDispatchPrepareId", "dbo.t_manu_SapMOrderProcessDispatchPrepare");
            DropIndex("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", new[] { "SapMOrderProcessDispatchPrepareId" });
            DropTable("dbo.t_manu_SapMOrderProcessDispatchPrepareStep");
            DropTable("dbo.t_manu_SapMOrderProcessDispatchPrepare");
        }
    }
}
