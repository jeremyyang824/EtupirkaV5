using System;
using System.Linq;
using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using Etupirka.Application.Portal;
using System.Web.Http;
using Etupirka.Application.Manufacture;
using Swashbuckle.Application;

namespace Etupirka.WebApi
{
    [DependsOn(typeof(AbpWebApiModule),
        //依赖的应用服务模块
        typeof(EtupirkaPortalApplicationModule),
        typeof(EtupirkaManufactureApplicationModule))]
    public class EtupirkaWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //需要自动创建WebApi的应用服务模块
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(EtupirkaPortalApplicationModule).Assembly, "portal")
                .Build();

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(EtupirkaManufactureApplicationModule).Assembly, "manufacture")
                .Build();

            //添加WebApi Filter，使用OWIN实现认证（Token类型：Bearer）
            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            //集成Swagger UI
            ConfigureSwaggerUi(() => new[]
            {
                "Etupirka.Application.Portal.xml",
                "Etupirka.WebApi.xml"
            });
        }

        private void ConfigureSwaggerUi(Func<string[]> configXmlDocs)
        {
            string[] docs = configXmlDocs?.Invoke();

            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", this.GetType().Namespace);
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());    //冲突处理

                    if (docs != null)
                        foreach (string xmlDoc in docs)
                        {
                            string docFile = $"{System.AppDomain.CurrentDomain.BaseDirectory}/bin/" + xmlDoc;
                            if (System.IO.File.Exists(docFile))
                                c.IncludeXmlComments(docFile);
                        }
                })
                .EnableSwaggerUi(c =>
                {
                    //注入XSRF头
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(EtupirkaWebApiModule)), "Etupirka.WebApi.Api.Scripts.Swagger-Custom.js");
                });
        }

    }
}

