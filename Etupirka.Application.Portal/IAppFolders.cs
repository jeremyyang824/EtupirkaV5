namespace Etupirka.Application.Portal
{
    /// <summary>
    /// 系统路径配置
    /// </summary>
    public interface IAppFolders : Abp.Dependency.ISingletonDependency
    {
        /// <summary>
        /// 临时文件夹
        /// </summary>
        string TempFileFolder { get; }

        /// <summary>
        /// 物料图片文件夹
        /// </summary>
        string ItemImagesFolder { get; }

        /// <summary>
        /// web日志文件夹
        /// </summary>
        string WebLogsFolder { get; set; }
    }
}