using System.Data.Entity.Migrations;
using Abp.Zero.EntityFramework;
using Abp.MultiTenancy;
using EntityFramework.DynamicFilters;
using Etupirka.EntityFramework.Portal.Migrations.SeedData;

namespace Etupirka.EntityFramework.Portal.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EtupirkaPortalDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Etupirka.Portal";
        }

        protected override void Seed(EtupirkaPortalDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                new CtmcOrganizationUnitsCreator(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
