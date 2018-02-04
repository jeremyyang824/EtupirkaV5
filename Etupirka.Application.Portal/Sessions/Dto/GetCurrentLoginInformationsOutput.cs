namespace Etupirka.Application.Portal.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }

        public UserOrganizationUnitInfoDto OrganizationUnit { get; set; }
    }
}