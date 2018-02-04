using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace Etupirka.Application.Portal.Sessions.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class UserOrganizationUnitInfoDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }
}
