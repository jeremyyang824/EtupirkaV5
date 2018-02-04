using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using Etupirka.Application.Portal.Users.Dto;

namespace Etupirka.Application.Portal.Users
{
    /// <summary>
    /// Host管理Teanant用户
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public class UserForHostAppService : EtupirkaAppServiceBase, IUserForHostAppService
    {
        private readonly IUserAppService _userAppService;

        public UserForHostAppService(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        /// <summary>
        /// 获取特定租户的用户信息
        /// </summary>
        public async Task<UserOutput> GetUser(long id, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return await _userAppService.GetUser(id);
            }
        }

        /// <summary>
        /// 获取特定租户的用户分页列表
        /// </summary>
        public async Task<PagedResultDto<UserOutput>> GetUsers(GetUsersInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                return await _userAppService.GetUsers(input);
            }
        }

        /// <summary>
        /// 分配用户角色
        /// </summary>
        public async Task AssignRoles(AssignRolesInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _userAppService.AssignRoles(input);
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public async Task CreateUser(CreateUserInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _userAppService.CreateUser(input);
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        public async Task UpdateUser(EditUserInput input, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _userAppService.UpdateUser(input);
            }
        }

        /// <summary>
        /// 重设用户密码
        /// </summary>
        public async Task ResetPassword(long id, int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await _userAppService.ResetPassword(id);
            }
        }
    }
}
