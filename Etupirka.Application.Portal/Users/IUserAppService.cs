using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Etupirka.Application.Portal.Users.Dto;

namespace Etupirka.Application.Portal.Users
{
    /// <summary>
    /// �û�����
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// ����ID��ȡ�û�
        /// </summary>
        /// <param name="id">�û�ID</param>
        Task<UserOutput> GetUser(long id);

        /// <summary>
        /// ��ȡ�û���ҳ�б�
        /// </summary>
        Task<PagedResultDto<UserOutput>> GetUsers(GetUsersInput input);

        /// <summary>
        /// �����û���ɫ
        /// </summary>
        Task AssignRoles(AssignRolesInput input);

        /// <summary>
        /// �����û�
        /// </summary>
        Task CreateUser(CreateUserInput input);

        /// <summary>
        /// �����û�
        /// </summary>
        Task UpdateUser(EditUserInput input);

        /// <summary>
        /// �����û�����
        /// </summary>
        Task ResetPassword(long id);

        /// <summary>
        /// �����û����Զ�����û���ѯ
        /// </summary>
        [DisableAuditing]
        Task<ListResultDto<UserOutput>> GetSuggestedUser(GetSuggestedUserInput input);
    }
}