using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Modules;
using Abp.Zero;
using Etupirka.Domain.External.Configurations;
using Etupirka.Domain.Portal;

namespace Etupirka.Domain.External
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(EtupirkaPortalDomainModule))]
    public class EtupirkaExternalDomainModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding setting providers
            Configuration.Settings.Providers.Add<ExternalAppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
