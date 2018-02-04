using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Abp.EntityFramework;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Manufacture.Configurations;

namespace Etupirka.EntityFramework.Manufacture
{
    public class EtupirkaManufactureDbContext : AbpDbContext
    {
        public IDbSet<HandOverBill> HandOverBillSet { get; set; }
        public IDbSet<HandOverBillLine> HandOverBillLineSet { get; set; }

        public IDbSet<ProcessCodeMap> ProcessCodeMapSet { get; set; }

        public IDbSet<SapWorkCenter> SapWorkCenterSet { get; set; }
        public IDbSet<SapMOrder> SapMOrderSet { get; set; }
        public IDbSet<SapMOrderProcess> SapMOrderProcessSet { get; set; }

        public IDbSet<SapMOrderProcessCooperate> SapMOrderProcessCooperateSet { get; set; }
        public IDbSet<SapMOrderProcessCooperateStep> SapMOrderProcessCooperateStepSet { get; set; }

        public IDbSet<SapMOrderProcessDispatchPrepare> SapMOrderProcessDispatchPrepareSet { get; set; }
        public IDbSet<SapMOrderProcessDispatchPrepareStep> SapMOrderProcessDispatchPrepareStepSet { get; set; }

        public IDbSet<SapSupplierMaper> SapSupplierMaperSet { get; set; }

        public IDbSet<DMESDispatchedWorker> DMESDispatchedWorkerSet { get; set; }

        public EtupirkaManufactureDbContext()
            : base("Default")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //EF Mapping Configuration
            modelBuilder.Configurations.Add(new OrderInfoConfiguration());
            modelBuilder.Configurations.Add(new OrderProcessConfiguration());
            modelBuilder.Configurations.Add(new HandOverDepartmentConfiguration());
            modelBuilder.Configurations.Add(new HandOverSupplierConfiguration());

            modelBuilder.Configurations.Add(new HandOverBillConfiguration());
            modelBuilder.Configurations.Add(new HandOverBillLineConfiguration());
            modelBuilder.Configurations.Add(new ProcessCodeMapConfiguration());

            modelBuilder.Configurations.Add(new SapWorkCenterConfiguration());
            modelBuilder.Configurations.Add(new SapMOrderConfiguration());
            modelBuilder.Configurations.Add(new SapMOrderProcessConfiguration());

            modelBuilder.Configurations.Add(new SapMOrderProcessCooperateConfiguration());
            modelBuilder.Configurations.Add(new SapMOrderProcessCooperateStepConfiguration());

            modelBuilder.Configurations.Add(new SapMOrderProcessDispatchPrepareConfiguration());
            modelBuilder.Configurations.Add(new SapMOrderProcessDispatchPrepareStepConfiguration());

            modelBuilder.Configurations.Add(new SapSupplierMaperConfiguration());

            modelBuilder.Configurations.Add(new DMESDispatchedWorkerConfiguration());

            //不使用复数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
