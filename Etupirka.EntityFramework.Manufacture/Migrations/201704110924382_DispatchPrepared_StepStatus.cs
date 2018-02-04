namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DispatchPrepared_StepStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStatus", c => c.Short());
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "Tooling_IsPreparedFinished", c => c.Short());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "Tooling_IsPreparedFinished", c => c.Boolean());
            DropColumn("dbo.t_manu_SapMOrderProcessDispatchPrepareStep", "StepStatus");
        }
    }
}
