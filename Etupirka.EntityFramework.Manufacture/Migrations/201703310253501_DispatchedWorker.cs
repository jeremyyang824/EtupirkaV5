namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DispatchedWorker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_manu_DMESDispatchedWorker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkerType = c.String(nullable: false, maxLength: 100),
                        WorkerStartDate = c.DateTime(nullable: false),
                        WorkerFinishDate = c.DateTime(),
                        IsWorkerSuccess = c.Boolean(nullable: false),
                        WorkerResultMessage = c.String(nullable: false, maxLength: 2000),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.t_manu_DMESDispatchedWorker");
        }
    }
}
