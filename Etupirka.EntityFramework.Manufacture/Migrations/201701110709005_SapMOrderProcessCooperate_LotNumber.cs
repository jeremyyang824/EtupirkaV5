namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapMOrderProcessCooperate_LotNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "LotNumber", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "LotNumber");
        }
    }
}
