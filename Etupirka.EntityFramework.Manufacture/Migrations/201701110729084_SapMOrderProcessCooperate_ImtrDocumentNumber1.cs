namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapMOrderProcessCooperate_ImtrDocumentNumber1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberPrefix", c => c.String(maxLength: 50));
            AlterColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberSerialNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberSerialNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.t_manu_SapMOrderProcessCooperate", "ImtrDocumentNumberPrefix", c => c.String());
        }
    }
}
