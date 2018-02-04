using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;

namespace Etupirka.Domain.External.Configurations
{
    /// <summary>
    /// 外部接口设置信息
    /// </summary>
    public class ExternalAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
            {
                //FSTI接口参数
                new SettingDefinition("FSTI_NetDomain", ""),
                new SettingDefinition("FSTI_NetUserName", "JDOper"),
                new SettingDefinition("FSTI_NetPassword", "stmc-y2c0j1x5"),
                new SettingDefinition("FSTI_Interface_SystemUid", "9999"),
                new SettingDefinition("FSTI_Interface_SystemPwd", "123456"),

                new SettingDefinition("BAPI_NetDomain", ""),
                new SettingDefinition("BAPI_NetUserName", "yy4612"),
                new SettingDefinition("BAPI_NetPassword", "120910"),
            };
        }
    }
}
