namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessPrepare2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchTicketID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_SapMOrderProcessDispatchPrepare", "DispatchTicketID", c => c.String(nullable: false));
        }
    }
}
