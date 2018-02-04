using System;
using System.Collections.Generic;
using System.Configuration;
using Abp.Configuration;

namespace Etupirka.Domain.Portal.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettings.DisplayLevel, "3", isVisibleToClients: true),
            };
        }
    }
}
