using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Etupirka.Application.Portal.Permissions.Dto;

namespace Etupirka.Application.Portal.Permissions
{
    /// <summary>
    /// 权限资源管理
    /// </summary>
    public class PermissionAppService : EtupirkaAppServiceBase, IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionAppService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 取得所有权限资源
        /// </summary>
        public ListResultDto<PermissionOutput> GetAllPermissions()
        {
            var permissions = _permissionManager.GetAllPermissions()
                .MapTo<List<PermissionOutput>>();
            return new ListResultDto<PermissionOutput>(permissions);
        }
    }
}
