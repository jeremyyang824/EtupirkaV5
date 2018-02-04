using System.Linq;
using Etupirka.Domain.Portal.MultiTenancy;

namespace Etupirka.EntityFramework.Portal.Migrations.SeedData
{
    public class CtmcTenantCreator
    {
        private readonly EtupirkaPortalDbContext _context;

        public CtmcTenantCreator(EtupirkaPortalDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            string ctmcName = "上海烟草机械有限公司";
            var ctmcTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == ctmcName);
            if (ctmcTenant == null)
            {
                _context.Tenants.Add(new SysTenant { TenancyName = ctmcName, Name = ctmcName });
                _context.SaveChanges();
            }
        }
    }
}
