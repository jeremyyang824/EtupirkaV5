using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Application.Portal.Users.Dto
{
    [AutoMap(typeof(SysUser))]
    public class EditUserInput
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(SysUser.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SysUser.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(SysUser.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(SysUser.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }
    }
}
