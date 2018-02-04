using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal.Permissions.Dto;

namespace Etupirka.Application.Portal.Permissions
{
    /// <summary>
    /// Host管理权限资源
    /// </summary>
    public interface IPermissionForHostAppService : IApplicationService
    {
        /// <summary>
        /// 取得所有权限资源
        /// </summary>
        ListResultDto<PermissionOutput> GetAllPermissions(int tenantId);
    }
}
