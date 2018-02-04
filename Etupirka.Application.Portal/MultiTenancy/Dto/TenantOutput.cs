using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Etupirka.Domain.Portal.MultiTenancy;

namespace Etupirka.Application.Portal.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(SysTenant))]
    public class TenantOutput : EntityDto, IPassivable, IHasCreationTime
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
    }
}