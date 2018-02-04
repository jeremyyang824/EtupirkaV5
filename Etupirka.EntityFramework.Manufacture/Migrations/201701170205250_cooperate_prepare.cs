namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cooperate_prepare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsPrepareFinished", c => c.Boolean());
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "HandOverQuantity", c => c.Decimal(precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "HandOverQuantity");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "IsPrepareFinished");
        }
    }
}
