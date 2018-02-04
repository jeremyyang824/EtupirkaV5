namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumn_process_price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcess", "ProcessPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.t_manu_SapMOrderProcess", "ProcessPriceUnit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcess", "ProcessPriceUnit");
            DropColumn("dbo.t_manu_SapMOrderProcess", "ProcessPrice");
        }
    }
}
