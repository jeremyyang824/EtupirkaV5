using System;
using System.Configuration;
using Abp.Owin;
using Etupirka.Web;
using Etupirka.WebApi.Api.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Etupirka.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.MapSignalR();

        }

        private static bool isTrue(string appSettingName)
        {
            return string.Equals(ConfigurationManager.AppSettings[appSettingName],
                "true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}