using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.Authorization.Roles;

namespace Etupirka.Application.Portal.Roles.Dto
{
    [AutoMap(typeof(SysRole))]
    public class EditRoleInput
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(SysRole.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SysRole.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }
    }
}
