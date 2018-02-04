using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.MultiTenancy;

namespace Etupirka.Application.Portal.MultiTenancy.Dto
{
    [AutoMapTo(typeof(SysTenant))]
    public class EditTenantInput
    {
        [Required]
        public int TenantId { get; set; }

        [Required]
        [StringLength(SysTenant.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(SysTenant.MaxNameLength)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
