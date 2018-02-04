using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class DMESDispatchedWorkerConfiguration : EntityBaseConfiguration<DMESDispatchedWorker, int>
    {
        public DMESDispatchedWorkerConfiguration()
        {
            ToTable("t_manu_DMESDispatchedWorker");

            Property(u => u.WorkerType).IsRequired().HasMaxLength(100);
            Property(u => u.WorkerStartDate).IsRequired();
            Property(u => u.WorkerFinishDate).IsOptional();
            Property(u => u.IsWorkerSuccess).IsRequired();
            Property(u => u.WorkerResultMessage).IsRequired();

        }
    }
}
