using System;
using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Etupirka.Domain.Manufacture;
using Etupirka.Domain.Portal;
using Etupirka.EntityFramework.Portal;
namespace Etupirka.EntityFramework.Manufacture
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule),
        typeof(EtupirkaPortalDomainModule),
        typeof(EtupirkaPortalDataModule),
        typeof(EtupirkaManufactureDomainModule))]
    public class EtupirkaManufactureDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<EtupirkaManufactureDbContext>(null);

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
