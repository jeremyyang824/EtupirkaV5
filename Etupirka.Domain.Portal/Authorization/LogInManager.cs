using System;
using System.Threading.Tasks;
using System.Transactions;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Timing;
using Abp.Zero.Configuration;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;
using Microsoft.AspNet.Identity;

namespace Etupirka.Domain.Portal.Authorization
{
    public class LogInManager : AbpLogInManager<SysTenant, SysRole, SysUser>
    {
        public LogInManager(
            SysUserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<SysTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig, IIocResolver iocResolver,
            SysRoleManager roleManager)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  roleManager)
        {
        }

        /// <summary>
        /// 取得登录结果
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="tenancyName">租户名</param>
        [UnitOfWork]
        public virtual async Task<AbpLoginResult<SysTenant, SysUser>> LoginAsync(string username, string tenancyName = null)
        {
            var result = await loginAsyncInternal(username, tenancyName);
            await SaveLoginAttempt(result, tenancyName, username);
            return result;
        }

        private async Task<AbpLoginResult<SysTenant, SysUser>> loginAsyncInternal(string username, string tenancyName = null)
        {
            if (username.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(username));
            }

            //Get and check tenant
            SysTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await this.getDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant?.Id;
            var user = await UserManager.AbpStore.FindByNameOrEmailAsync(tenantId, username);
            if (user == null)
            {
                return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
            }

            this.UserManager.InitializeLockoutSettings(tenantId);
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.LockedOut, tenant, user);
            }

            return await CreateLoginResultAsync(user, tenant);
        }

        private async Task<AbpLoginResult<SysTenant, SysUser>> CreateLoginResultAsync(SysUser user, SysTenant tenant = null)
        {
            if (!user.IsActive)
            {
                return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.UserIsNotActive);
            }

            if (await isEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new AbpLoginResult<SysTenant, SysUser>(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await UserManager.AbpStore.UpdateAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return new AbpLoginResult<SysTenant, SysUser>(
                tenant,
                user,
                await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)
            );
        }

        private async Task<SysTenant> getDefaultTenantAsync()
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == AbpTenant<SysUser>.DefaultTenantName);
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }
            return tenant;
        }

        private async Task<bool> tryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    (await UserManager.AccessFailedAsync(userId)).CheckErrors();
                    var isLockOut = await UserManager.IsLockedOutAsync(userId);
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();
                    return isLockOut;
                }
            }
        }

        private async Task<bool> isEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }

        private async Task SaveLoginAttempt(AbpLoginResult<SysTenant, SysUser> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant?.Id;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User?.Id,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }
    }
}
