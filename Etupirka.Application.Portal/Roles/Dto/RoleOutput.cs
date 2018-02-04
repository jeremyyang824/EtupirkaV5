using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Etupirka.Domain.Portal.Authorization.Roles;

namespace Etupirka.Application.Portal.Roles.Dto
{
    [AutoMapFrom(typeof(SysRole))]
    public class RoleOutput : EntityDto, IHasCreationTime
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
