using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Application.Portal.Users.Dto
{
    [AutoMap(typeof(SysUser))]
    public class CreateUserInput
    {
        [Required]
        [StringLength(SysUser.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(SysUser.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SysUser.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}