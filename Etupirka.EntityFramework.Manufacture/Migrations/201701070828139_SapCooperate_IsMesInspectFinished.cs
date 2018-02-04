namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapCooperate_IsMesInspectFinished : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsMesInspectFinished", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsMesInspectFinished");
        }
    }
}
