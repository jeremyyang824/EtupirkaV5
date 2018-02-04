using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Portal.Users.Dto
{
    public class AssignRolesInput
    {
        [Range(1, int.MaxValue)]
        public long UserId { get; set; }

        [Required]
        public string[] AssignedRoleNames { get; set; }
    }
}
