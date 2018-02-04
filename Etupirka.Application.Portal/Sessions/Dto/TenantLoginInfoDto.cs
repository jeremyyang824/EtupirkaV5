using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Portal.MultiTenancy;

namespace Etupirka.Application.Portal.Sessions.Dto
{
    [AutoMapFrom(typeof(SysTenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}