namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fspointofuse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_manu_SapSupplierMaper",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SapSupplierCode = c.String(nullable: false, maxLength: 50),
                        FsPointOfUse = c.String(nullable: false, maxLength: 50),
                        SupplierName = c.String(nullable: false, maxLength: 100),
                        IsFsSupplier = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "CooperaterFsPointOfUse", c => c.String(maxLength: 50));
            AddColumn("dbo.t_manu_SapMOrderProcess", "LIFNR", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcess", "LIFNR");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "CooperaterFsPointOfUse");
            DropTable("dbo.t_manu_SapSupplierMaper");
        }
    }
}
