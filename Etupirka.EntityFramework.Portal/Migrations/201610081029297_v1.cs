namespace Etupirka.EntityFramework.Portal.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpUserClaims",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_sys_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.t_sys_Users", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.t_sys_Users", "AccessFailedCount", c => c.Int(nullable: false));
            AddColumn("dbo.t_sys_Users", "IsLockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.t_sys_Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.t_sys_Users", "IsPhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.t_sys_Users", "SecurityStamp", c => c.String());
            AddColumn("dbo.t_sys_Users", "IsTwoFactorEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpUserClaims", "UserId", "dbo.t_sys_Users");
            DropIndex("dbo.AbpUserClaims", new[] { "UserId" });
            DropColumn("dbo.t_sys_Users", "IsTwoFactorEnabled");
            DropColumn("dbo.t_sys_Users", "SecurityStamp");
            DropColumn("dbo.t_sys_Users", "IsPhoneNumberConfirmed");
            DropColumn("dbo.t_sys_Users", "PhoneNumber");
            DropColumn("dbo.t_sys_Users", "IsLockoutEnabled");
            DropColumn("dbo.t_sys_Users", "AccessFailedCount");
            DropColumn("dbo.t_sys_Users", "LockoutEndDateUtc");
            DropTable("dbo.AbpUserClaims",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
