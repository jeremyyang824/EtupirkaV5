namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class porelease : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsSapPoRequestReleased", c => c.Boolean());
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "SapPoRequestNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsSapPoReleased", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsSapPoReleased");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "SapPoRequestNumber");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsSapPoRequestReleased");
        }
    }
}
