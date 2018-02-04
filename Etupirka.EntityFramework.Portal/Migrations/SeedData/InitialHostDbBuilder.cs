using EntityFramework.DynamicFilters;

namespace Etupirka.EntityFramework.Portal.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly EtupirkaPortalDbContext _context;

        public InitialHostDbBuilder(EtupirkaPortalDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
