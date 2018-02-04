using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Application.Portal.MultiTenancy.Dto
{
    [AutoMapTo(typeof(SysTenant))]
    public class CreateTenantInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(SysTenant.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(SysTenant.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SysUser.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }
    }
}