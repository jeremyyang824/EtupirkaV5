using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.Authorization.Roles;

namespace Etupirka.Application.Portal.Roles.Dto
{
    [AutoMap(typeof(SysRole))]
    public class CreateRoleInput
    {
        [Required]
        [StringLength(SysRole.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SysRole.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }
    }
}
