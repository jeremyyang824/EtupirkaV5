using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Events.Bus;
using Abp.IdentityFramework;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Etupirka.Application.Portal.Dto;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;
using Microsoft.AspNet.Identity;

namespace Etupirka.Application.Portal
{
    /// <summary>
    /// 应用服务基类
    /// </summary>
    public abstract class EtupirkaAppServiceBase : ApplicationService
    {
        /// <summary>
        /// 系统租户管理
        /// </summary>
        public SysTenantManager TenantManager { get; set; }

        /// <summary>
        /// 系统用户管理
        /// </summary>
        public SysUserManager UserManager { get; set; }

        /// <summary>
        /// 系统组织机构管理
        /// </summary>
        public OrganizationUnitManager OrganizationUnitManager { get; set; }

        /// <summary>
        /// 事件总线
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        /// GUID生成器
        /// </summary>
        public IGuidGenerator GuidGenerator { get; set; }

        /// <summary>
        /// 系统路径配置
        /// </summary>
        public IAppFolders AppFolders { get; set; }

        protected EtupirkaAppServiceBase()
        {
            LocalizationSourceName = EtupirkaPortalConsts.LocalizationSourceName;
            EventBus = NullEventBus.Instance;
        }

        /// <summary>
        /// 获取当前用户实体（非空）
        /// </summary>
        protected virtual async Task<SysUser> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }
            return user;
        }

        /// <summary>
        /// 获取当前用户所属部门（非空）
        /// </summary>
        protected virtual async Task<OrganizationUnit> GetCurrentUserOrganizationUnitAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }
            var organizationUnit = (await this.UserManager.GetOrganizationUnitsAsync(user))?.FirstOrDefault();
            if (organizationUnit == null)
            {
                throw new UserFriendlyException($"用户[{user.Surname}]未配置部门！");
            }
            return organizationUnit;
        }

        /// <summary>
        /// 获取当前租户实体（Host为null）
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<SysTenant> GetCurrentTenantAsync()
        {
            return await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 导出到临时文件夹文件
        /// </summary>
        protected FileDto SaveToTempFolder(MemoryStream ms, string fileName, string fileType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName");
            if (string.IsNullOrWhiteSpace(fileType))
                throw new ArgumentNullException("fileType");

            FileDto fileDto = new FileDto(fileName, fileType);
            string fullPath = Path.Combine(AppFolders.TempFileFolder, fileDto.FileToken);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
                fs.Close();
            }
            return fileDto;
        }

        /// <summary>
        /// 取得临时文件完整路径
        /// </summary>
        protected FileDisposer GetTempFile(FileDto fileDto)
        {
            string fullPath = Path.Combine(AppFolders.TempFileFolder, fileDto.FileToken);
            FileInfo fileInfo = new FileInfo(fullPath);
            return new FileDisposer(fileInfo);
        }

        /// <summary>
        /// 自动删除文件
        /// </summary>
        protected sealed class FileDisposer : IDisposable
        {
            /// <summary>
            /// 文件信息
            /// </summary>
            public FileInfo File { get; private set; }

            public FileDisposer(FileInfo file)
            {
                if (file == null || !file.Exists)
                    throw new UserFriendlyException($"临时文件不存在！");
                this.File = file;
            }

            public void Dispose()
            {
                try
                {
                    File.Delete();
                }
                catch
                {
                    //文件删除失败
                }
            }
        }
    }
}
