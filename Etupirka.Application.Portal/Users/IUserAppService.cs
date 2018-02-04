using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Etupirka.Application.Portal.Users.Dto;

namespace Etupirka.Application.Portal.Users
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="id">用户ID</param>
        Task<UserOutput> GetUser(long id);

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        Task<PagedResultDto<UserOutput>> GetUsers(GetUsersInput input);

        /// <summary>
        /// 分配用户角色
        /// </summary>
        Task AssignRoles(AssignRolesInput input);

        /// <summary>
        /// 创建用户
        /// </summary>
        Task CreateUser(CreateUserInput input);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task UpdateUser(EditUserInput input);

        /// <summary>
        /// 重设用户密码
        /// </summary>
        Task ResetPassword(long id);

        /// <summary>
        /// 根据用户名自动完成用户查询
        /// </summary>
        [DisableAuditing]
        Task<ListResultDto<UserOutput>> GetSuggestedUser(GetSuggestedUserInput input);
    }
}