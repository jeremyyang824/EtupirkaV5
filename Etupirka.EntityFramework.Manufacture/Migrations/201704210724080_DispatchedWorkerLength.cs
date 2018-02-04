namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DispatchedWorkerLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_manu_DMESDispatchedWorker", "WorkerResultMessage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_manu_DMESDispatchedWorker", "WorkerResultMessage", c => c.String(nullable: false, maxLength: 2000));
        }
    }
}
