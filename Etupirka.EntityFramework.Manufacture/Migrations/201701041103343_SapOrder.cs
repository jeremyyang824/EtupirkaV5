namespace Etupirka.EntityFramework.Manufacture.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SapOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_meta_ProcessCodeMap",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SapProcessCode = c.String(nullable: false, maxLength: 50),
                        SapProcessName = c.String(nullable: false, maxLength: 200),
                        FsAuxiProcessCode = c.String(nullable: false, maxLength: 50),
                        FsWorkProcessCode = c.String(nullable: false, maxLength: 50),
                        FsProcessName = c.String(nullable: false, maxLength: 200),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_manu_SapMOrderProcess",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SapMOrderId = c.Guid(nullable: false),
                        RoutingNumber = c.String(nullable: false, maxLength: 50),
                        OrderCounter = c.Int(nullable: false),
                        OperationNumber = c.String(nullable: false, maxLength: 50),
                        OperationCtrlCode = c.String(nullable: false, maxLength: 50),
                        ProductionPlant = c.String(nullable: false, maxLength: 50),
                        WorkCenterObjId = c.String(nullable: false, maxLength: 50),
                        WorkCenterCode = c.String(nullable: false, maxLength: 50),
                        WorkCenterName = c.String(nullable: false, maxLength: 100),
                        StandardText = c.String(nullable: false, maxLength: 100),
                        ProcessText1 = c.String(nullable: false, maxLength: 500),
                        ProcessText2 = c.String(nullable: false, maxLength: 500),
                        UMREN = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UMREZ = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Unit = c.String(nullable: false, maxLength: 50),
                        BaseQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProcessQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ScrapQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE01 = c.String(nullable: false, maxLength: 50),
                        VGW01 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE02 = c.String(nullable: false, maxLength: 50),
                        VGW02 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE03 = c.String(nullable: false, maxLength: 50),
                        VGW03 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE04 = c.String(nullable: false, maxLength: 50),
                        VGW04 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE05 = c.String(nullable: false, maxLength: 50),
                        VGW05 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VGE06 = c.String(nullable: false, maxLength: 50),
                        VGW06 = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ScheduleStartDate = c.DateTime(),
                        ScheduleFinishDate = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_manu_SapMOrder", t => t.SapMOrderId)
                .Index(t => t.SapMOrderId);
            
            CreateTable(
                "dbo.t_manu_SapMOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        ProductionPlant = c.String(nullable: false, maxLength: 50),
                        MRPController = c.String(nullable: false, maxLength: 50),
                        ProductionScheduler = c.String(nullable: false, maxLength: 50),
                        MaterialNumber = c.String(nullable: false, maxLength: 100),
                        MaterialDescription = c.String(nullable: false, maxLength: 200),
                        MaterialExternal = c.String(maxLength: 500),
                        MaterialGuid = c.String(maxLength: 50),
                        MaterialVersion = c.String(maxLength: 50),
                        RoutingNumber = c.String(maxLength: 50),
                        ScheduleReleaseDate = c.DateTime(),
                        ActualReleaseDate = c.DateTime(),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        ProductionStartDate = c.DateTime(),
                        ProductionFinishDate = c.DateTime(),
                        ActualStartDate = c.DateTime(),
                        ActualFinishDate = c.DateTime(),
                        TargetQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ScrapQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ConfirmedQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Unit = c.String(nullable: false, maxLength: 50),
                        UnitISO = c.String(maxLength: 50),
                        Priority = c.String(maxLength: 50),
                        OrderType = c.String(maxLength: 50),
                        WBSElement = c.String(maxLength: 50),
                        SystemStatus = c.String(maxLength: 100),
                        Batch = c.String(maxLength: 50),
                        ABLAD = c.String(maxLength: 50),
                        WEMPF = c.String(maxLength: 50),
                        AufkAenam = c.String(maxLength: 50),
                        AufkAedat = c.DateTime(),
                        AufkPhas0 = c.String(maxLength: 20),
                        AufkPhas1 = c.String(maxLength: 20),
                        AufkPhas2 = c.String(maxLength: 20),
                        AufkPhas3 = c.String(maxLength: 20),
                        SapId = c.Int(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_manu_SapWorkCenter",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WorkCenterCode = c.String(nullable: false, maxLength: 50),
                        WorkCenterName = c.String(nullable: false, maxLength: 100),
                        ProductionPlant = c.String(nullable: false, maxLength: 50),
                        OBJTY = c.String(nullable: false, maxLength: 50),
                        OBJID = c.String(nullable: false, maxLength: 50),
                        PLANV = c.String(maxLength: 50),
                        VERWE = c.String(maxLength: 50),
                        XSPRR = c.String(maxLength: 50),
                        LVORM = c.String(maxLength: 50),
                        KAPAR = c.String(maxLength: 50),
                        MEINS = c.String(maxLength: 50),
                        AEDAT_GRND = c.DateTime(),
                        AENAM_GRND = c.String(maxLength: 50),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_manu_SapMOrderProcess", "SapMOrderId", "dbo.t_manu_SapMOrder");
            DropIndex("dbo.t_manu_SapMOrderProcess", new[] { "SapMOrderId" });
            DropTable("dbo.t_manu_SapWorkCenter");
            DropTable("dbo.t_manu_SapMOrder");
            DropTable("dbo.t_manu_SapMOrderProcess");
            DropTable("dbo.t_meta_ProcessCodeMap");
        }
    }
}
