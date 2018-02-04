namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapMOrderProcessCooperate_ImtrDocumentNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberPrefix", c => c.String());
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberSerialNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberSerialNumber");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberPrefix");
        }
    }
}
