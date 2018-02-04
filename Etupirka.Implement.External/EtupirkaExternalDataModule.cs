using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Modules;
using Etupirka.Domain.External;
using Etupirka.Domain.Portal;

namespace Etupirka.Implement.External
{
    [DependsOn(typeof(EtupirkaExternalDomainModule), typeof(EtupirkaPortalDomainModule))]
    public class EtupirkaExternalDataModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
