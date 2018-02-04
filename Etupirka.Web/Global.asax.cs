using System;
using System.Web.Helpers;
using Abp.Castle.Logging.Log4Net;
using Abp.Configuration;
using Abp.Web;
using Castle.Facilities.Logging;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;

namespace Etupirka.Web
{
    public class Global : AbpWebApplication<EtupirkaWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //启用Log4net并配置
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            //EF6 Profiler
            MiniProfilerEF6.Initialize();

            base.Application_Start(sender, e);
        }

        protected override void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);
        }

        protected void Application_BeginRequest()
        {
            ISettingManager setting = AbpBootstrapper.IocManager.Resolve<ISettingManager>();
            bool isDevMode = setting.GetSettingValue<bool>(AppSettings.General.IsDevMode);
            if (isDevMode)
            {
                MiniProfiler.Start();
            }
        }
        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}