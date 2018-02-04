using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Etupirka.Application.Manufacture;
using Etupirka.Application.Portal;
using Etupirka.EntityFramework.Manufacture;
using Etupirka.EntityFramework.Portal;
using Etupirka.Implement.External;
using Etupirka.WebApi;

namespace Etupirka.Web
{
    [DependsOn(
        //持久化模块 -> 领域模型模块、AbpZeroEntityFrameworkModule
        typeof(EtupirkaPortalDataModule),
        typeof(EtupirkaManufactureDataModule),
        typeof(EtupirkaExternalDataModule),
        //应用服务模块 -> 领域模型模块、AbpAutoMapperModule
        typeof(EtupirkaPortalApplicationModule),
        typeof(EtupirkaManufactureApplicationModule),
        //其他依赖模块
        typeof(EtupirkaWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpWebMvcModule))]
    public class EtupirkaWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //系统配置
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //启用基于数据库的本地化管理
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<EtupirkaNavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Area配置
            AreaRegistration.RegisterAllAreas();
            //路由配置
            GlobalConfiguration.Configure(WebApiConfig.Register);
            MvcRouteConfig.RegisterRoutes(RouteTable.Routes);
            //资源Bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.ItemImagesFolder = server.MapPath("~/Uploads/ItemImages");
            appFolders.TempFileFolder = server.MapPath("~/Uploads/Temp");
            appFolders.WebLogsFolder = server.MapPath("~/Logs");

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.ItemImagesFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.WebLogsFolder);
            } catch { }
        }
    }
}