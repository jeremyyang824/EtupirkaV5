using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal.Roles.Dto;

namespace Etupirka.Application.Portal.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        /// <param name="id">角色ID</param>
        Task<RoleOutput> GetRole(int id);

        /// <summary>
        /// 取得角色的权限
        /// </summary>
        /// <param name="id">角色ID</param>
        Task<string[]> GetRolePermissions(int id);

        /// <summary>
        /// 取得所有角色
        /// </summary>
        Task<ListResultDto<RoleOutput>> GetRoles();

        /// <summary>
        /// 更新角色权限
        /// </summary>
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);

        /// <summary>
        /// 创建新角色
        /// </summary>
        Task CreateRole(CreateRoleInput input);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        Task UpdateRole(EditRoleInput input);

        /// <summary>
        /// 删除角色
        /// </summary>
        Task DeleteRole(int id);
    }
}
