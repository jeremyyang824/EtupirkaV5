using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Localization;
using Microsoft.AspNet.Identity;

namespace Etupirka.Domain.Portal.Users
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysUser : AbpUser<SysUser>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static SysUser CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new SysUser
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = LocalizationHelper.GetString(EtupirkaPortalConsts.LocalizationSourceName, "Admin"),
                Surname = LocalizationHelper.GetString(EtupirkaPortalConsts.LocalizationSourceName, "Admin"),
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}
