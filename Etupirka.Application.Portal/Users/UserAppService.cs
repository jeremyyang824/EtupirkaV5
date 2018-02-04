using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using Etupirka.Application.Portal.Users.Dto;
using Etupirka.Domain.Portal.Authorization;
using Etupirka.Domain.Portal.Notifications;
using Etupirka.Domain.Portal.Users;
using Microsoft.AspNet.Identity;

namespace Etupirka.Application.Portal.Users
{
    /// <summary>
    /// �û�����
    /// </summary>
    //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UserAppService : EtupirkaAppServiceBase, IUserAppService
    {
        private readonly IRepository<SysUser, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IEtupirkaPortalNotifier _etupirkaPortalNotifier;

        public UserAppService(IRepository<SysUser, long> userRepository,
            IPermissionManager permissionManager, IEtupirkaPortalNotifier etupirkaPortalNotifier)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _etupirkaPortalNotifier = etupirkaPortalNotifier;
        }

        /// <summary>
        /// ����ID��ȡ�û�
        /// </summary>
        /// <param name="id">�û�ID</param>
        public async Task<UserOutput> GetUser(long id)
        {
            var user = await this.UserManager.GetUserByIdAsync(id);
            return user.MapTo<UserOutput>();
        }

        /// <summary>
        /// ��ȡ�û���ҳ�б�
        /// </summary>
        public async Task<PagedResultDto<UserOutput>> GetUsers(GetUsersInput input)
        {
            var query = UserManager.Users
                .Include(u => u.Roles)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                )
                .WhereIf(
                    input.IsActive.HasValue,
                    t => t.IsActive == input.IsActive.Value
                );

            var userCount = await query.CountAsync();
            var users = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var l = new PagedResultDto<UserOutput>(
                userCount,
                users.MapTo<List<UserOutput>>()
                );
            return l;
        }

        /// <summary>
        /// �����û���ɫ
        /// </summary>
        public async Task AssignRoles(AssignRolesInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new UserFriendlyException($"�û�ID��[{input.UserId}]�����ڣ�");

            CheckErrors(await UserManager.SetRoles(user, input.AssignedRoleNames));

        }

        /// <summary>
        /// �����û�
        /// </summary>
        public async Task CreateUser(CreateUserInput input)
        {
            var user = input.MapTo<SysUser>();
            user.TenantId = AbpSession.TenantId; //��ǰ�⻧
            user.Password = new PasswordHasher().HashPassword(SysUser.DefaultPassword); //Ĭ������
            user.IsActive = true;

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //֪ͨ
            await this._etupirkaPortalNotifier.WelcomeToTheApplicationAsync(user);
        }

        /// <summary>
        /// �����û�
        /// </summary>
        public async Task UpdateUser(EditUserInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            if (user == null)
                throw new UserFriendlyException($"�û�ID��[{input.Id}]�����ڣ�");

            input.MapTo(user);
            CheckErrors(await UserManager.UpdateAsync(user));

            //֪ͨ
            await this._etupirkaPortalNotifier.WelcomeToTheApplicationAsync(user);
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        public async Task ResetPassword(long id)
        {
            var user = await UserManager.GetUserByIdAsync(id);
            if (user == null)
                throw new UserFriendlyException($"�û�ID��[{id}]�����ڣ�");

            user.Password = new PasswordHasher().HashPassword(SysUser.DefaultPassword); //Ĭ������
            CheckErrors(await UserManager.UpdateAsync(user));
        }

        /// <summary>
        /// �����û����Զ�����û���ѯ
        /// </summary>
        [DisableAuditing]
        public async Task<ListResultDto<UserOutput>> GetSuggestedUser(GetSuggestedUserInput input)
        {
            var users = await UserManager.Users
                .Include(u => u.Roles)
                .Where(u => u.UserName.StartsWith(input.UserNamePrefix))
                .Take(input.ReturnCount)
                .ToListAsync();

            return new ListResultDto<UserOutput>(users.MapTo<List<UserOutput>>());
        }

        //public async Task ProhibitPermission(ProhibitPermissionInput input)
            //{
            //    var user = await UserManager.GetUserByIdAsync(input.UserId);
            //    var permission = _permissionManager.GetPermission(input.PermissionName);

            //    await UserManager.ProhibitPermissionAsync(user, permission);
            //}

            //public async Task RemoveFromRole(long userId, string roleName)
            //{
            //    CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
            //}



            //public async Task CreateUser(CreateUserInput input)
            //{
            //    var user = input.MapTo<SysUser>();

            //    user.TenantId = AbpSession.TenantId;
            //    user.Password = new PasswordHasher().HashPassword(input.Password);
            //    user.IsEmailConfirmed = true;

            //    CheckErrors(await UserManager.CreateAsync(user));
            //}
        }
}