using Abp.Dependency;
using Etupirka.Application.Portal;

namespace Etupirka.Web
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        /// <summary>
        /// 临时文件夹
        /// </summary>
        public string TempFileFolder { get; set; }

        /// <summary>
        /// 物料图片文件夹
        /// </summary>
        public string ItemImagesFolder { get; set; }

        /// <summary>
        /// web日志文件夹
        /// </summary>
        public string WebLogsFolder { get; set; }
    }
}