using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Etupirka.Domain.Portal;

namespace Etupirka.EntityFramework.Portal
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(EtupirkaPortalDomainModule))]
    public class EtupirkaPortalDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EtupirkaPortalDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
   