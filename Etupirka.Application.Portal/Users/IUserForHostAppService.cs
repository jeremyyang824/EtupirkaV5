using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal.Users.Dto;

namespace Etupirka.Application.Portal.Users
{
    /// <summary>
    /// Host管理Teanant用户
    /// </summary>
    public interface IUserForHostAppService : IApplicationService
    {
        /// <summary>
        /// 获取特定租户的用户信息
        /// </summary>
        Task<UserOutput> GetUser(long id, int tenantId);

        /// <summary>
        /// 获取特定租户的用户分页列表
        /// </summary>
        Task<PagedResultDto<UserOutput>> GetUsers(GetUsersInput input, int tenantId);

        /// <summary>
        /// 分配用户角色
        /// </summary>
        Task AssignRoles(AssignRolesInput input, int tenantId);

        /// <summary>
        /// 创建用户
        /// </summary>
        Task CreateUser(CreateUserInput input, int tenantId);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task UpdateUser(EditUserInput input, int tenantId);

        /// <summary>
        /// 重设用户密码
        /// </summary>
        Task ResetPassword(long id, int tenantId);
    }
}
