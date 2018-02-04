namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sapapi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "SapPoLine", c => c.String(maxLength: 50));
            AddColumn("dbo.t_manu_SapMOrderProcess", "BANFN", c => c.String(maxLength: 50));
            AddColumn("dbo.t_manu_SapMOrderProcess", "BNFPO", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcess", "BNFPO");
            DropColumn("dbo.t_manu_SapMOrderProcess", "BANFN");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "SapPoLine");
        }
    }
}
