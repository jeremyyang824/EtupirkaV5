using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using Etupirka.Application.Portal.Permissions.Dto;

namespace Etupirka.Application.Portal.Permissions
{
    /// <summary>
    /// Host管理权限资源
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public class PermissionForHostAppService : EtupirkaAppServiceBase, IPermissionForHostAppService
    {
        private readonly IPermissionAppService _permissionAppService;

        public PermissionForHostAppService(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }

        /// <summary>
        /// 取得所有权限资源
        /// </summary>
        public ListResultDto<PermissionOutput> GetAllPermissions(int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return _permissionAppService.GetAllPermissions();
            }
        }
    }
}
