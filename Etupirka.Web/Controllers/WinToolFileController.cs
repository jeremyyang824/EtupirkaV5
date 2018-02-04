using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Configuration;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Web.Controllers
{
    [DisableAuditing]
    public class WinToolFileController : Controller
    {
        private readonly ISettingManager _settingManager;
        public WinToolFileController(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }

        /// <summary>
        /// 获取网络物理文件
        /// \\SERVERSVN\Shared\test.txt
        /// </summary>
        public FileResult GetNetworkFile(string filepath, string filename = "")
        {
            //if (string.IsNullOrWhiteSpace(filepath))
            //    throw new ArgumentNullException(nameof(filepath));

            //WebRequest request = WebRequest.Create(filepath);
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            ////request.Credentials = this.getNetworkCredential();

            //var response = request.GetResponse();
            //return File(response.GetResponseStream(), response.ContentType);

            string server = this._settingManager.GetSettingValue("WinToolServer");
            string username = this._settingManager.GetSettingValue("WinToolServerUid");
            string password = this._settingManager.GetSettingValue("WinToolServerPwd");
            FileStream fs = null;
            var contentType = MimeMapping.GetMimeMapping(filepath);
            using (IdentityScope scope = new IdentityScope(username, server, password))
            {
                fs = System.IO.File.OpenRead(filepath);
            }
            return File(fs, contentType);
        }
    }
}