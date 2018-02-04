using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Modules;
using Abp.Zero;
using Etupirka.Domain.Manufacture.Notifications;
using Etupirka.Domain.Portal;

namespace Etupirka.Domain.Manufacture
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(EtupirkaPortalDomainModule))]
    public class EtupirkaManufactureDomainModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding notification providers
            Configuration.Notifications.Providers.Add<EtupirkaManufactureNotificationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
