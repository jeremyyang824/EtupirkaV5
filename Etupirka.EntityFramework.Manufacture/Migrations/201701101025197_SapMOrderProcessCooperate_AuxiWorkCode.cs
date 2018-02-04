namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapMOrderProcessCooperate_AuxiWorkCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "FsAuxiProcessCode", c => c.String(maxLength: 50));
            AddColumn("dbo.t_manu_SapMOrderProcessCooperate", "FsWorkProcessCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "FsWorkProcessCode");
            DropColumn("dbo.t_manu_SapMOrderProcessCooperate", "FsAuxiProcessCode");
        }
    }
}
