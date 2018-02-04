using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Configuration;

namespace Etupirka.Web.Controllers
{
    [DisableAuditing]
    public class WinchillDrawingController : Controller
    {
        private readonly ISettingManager _settingManager;
        public WinchillDrawingController(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }

        public FileResult GetClientDrawing(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            WebRequest request = WebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add("Authorization", this.getAuthorization());

            var response = request.GetResponse();
            return File(response.GetResponseStream(), response.ContentType);
        }

        private string getAuthorization()
        {
            string username = this._settingManager.GetSettingValue("WinchillUid");
            string password = this._settingManager.GetSettingValue("WinchillPwd");
            string authorization = $"{username}:{password}";
            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        }
    }
}