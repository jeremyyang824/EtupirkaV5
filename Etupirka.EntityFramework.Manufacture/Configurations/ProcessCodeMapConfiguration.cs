using System;
using System.Collections.Generic;
using System.Linq;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class ProcessCodeMapConfiguration : EntityBaseConfiguration<ProcessCodeMap, int>
    {
        public ProcessCodeMapConfiguration()
        {
            ToTable("t_meta_ProcessCodeMap");

            Property(u => u.SapProcessCode).IsRequired().HasMaxLength(50);
            Property(u => u.SapProcessName).IsRequired().HasMaxLength(200);
            Property(u => u.FsAuxiProcessCode).IsRequired().HasMaxLength(50);
            Property(u => u.FsWorkProcessCode).IsRequired().HasMaxLength(50);
            Property(u => u.FsProcessName).IsRequired().HasMaxLength(200);

        }
    }
}
