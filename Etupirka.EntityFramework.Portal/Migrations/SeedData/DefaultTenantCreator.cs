using System.Linq;
using Etupirka.Domain.Portal.MultiTenancy;

namespace Etupirka.EntityFramework.Portal.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly EtupirkaPortalDbContext _context;

        public DefaultTenantCreator(EtupirkaPortalDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == SysTenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new SysTenant { TenancyName = SysTenant.DefaultTenantName, Name = SysTenant.DefaultTenantName });
                _context.SaveChanges();
            }
        }
    }
}
