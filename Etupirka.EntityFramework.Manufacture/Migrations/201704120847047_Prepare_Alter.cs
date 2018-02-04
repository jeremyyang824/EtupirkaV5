namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prepare_Alter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchWorKTicketID", c => c.Int(nullable: false));
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "NC_RequiredDate", c => c.DateTime());
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "Tooling_RequiredDate", c => c.DateTime());
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "NC_IsPreparedFinished", c => c.Short());
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchTicketID");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStatus");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStartDate");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepRequiredDate");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepFinishDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepFinishDate", c => c.DateTime());
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepRequiredDate", c => c.DateTime());
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStartDate", c => c.DateTime());
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStatus", c => c.Short());
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchTicketID", c => c.Int(nullable: false));
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "NC_IsPreparedFinished", c => c.Boolean());
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "Tooling_RequiredDate");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "NC_RequiredDate");
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchWorKTicketID");
        }
    }
}
