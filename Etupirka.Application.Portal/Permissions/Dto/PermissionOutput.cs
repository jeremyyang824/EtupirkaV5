using System;
using System.Collections.Generic;
using Abp.Authorization;
using Abp.AutoMapper;

namespace Etupirka.Application.Portal.Permissions.Dto
{
    [AutoMapFrom(typeof(Permission))]
    public class PermissionOutput
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
