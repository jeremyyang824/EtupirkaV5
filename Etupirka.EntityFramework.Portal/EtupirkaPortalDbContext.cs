using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Abp.Zero.EntityFramework;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.EntityFramework.Portal
{
    public class EtupirkaPortalDbContext : AbpZeroDbContext<SysTenant, SysRole, SysUser>
    {
        public EtupirkaPortalDbContext()
            : base("Default")
        {

        }

        public EtupirkaPortalDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public EtupirkaPortalDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //不使用复数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //系统表前缀
            modelBuilder.ChangeAbpTablePrefix<SysTenant, SysRole, SysUser>("t_sys_");
        }
    }
}
