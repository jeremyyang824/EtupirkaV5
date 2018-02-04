namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billLine3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_manu_HandOverBill", "BillCodeSerialNumber", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_HandOverBill", "BillCodeSerialNumber", c => c.Int(nullable: false));
        }
    }
}
