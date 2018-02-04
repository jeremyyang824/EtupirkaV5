using System.Collections.Generic;
using System.Configuration;
using Abp.Configuration;

namespace Etupirka.Web
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettings.General.IsDevMode, ConfigurationManager.AppSettings[AppSettings.General.IsDevMode] ?? "false"),

                new SettingDefinition("WinchillUid", "lff"),
                new SettingDefinition("WinchillPwd", "lff"),

                new SettingDefinition("WinToolServer", "SERVERSVN"),
                new SettingDefinition("WinToolServerUid", "Administrator"),
                new SettingDefinition("WinToolServerPwd", "SH-ctmc"),
            };
        }
    }

    /// <summary>
    /// 配置键
    /// </summary>
    public static class AppSettings
    {
        public static class General
        {
            public const string IsDevMode = "App.General.IsDevMode";
        }
    }
}