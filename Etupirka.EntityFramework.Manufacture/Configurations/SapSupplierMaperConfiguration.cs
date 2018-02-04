using System;
using System.Collections.Generic;
using System.Linq;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class SapSupplierMaperConfiguration : EntityBaseConfiguration<SapSupplierMaper, int>
    {
        public SapSupplierMaperConfiguration()
        {
            ToTable("t_manu_SapSupplierMaper");

            Property(u => u.SapSupplierCode).IsRequired().HasMaxLength(50);
            Property(u => u.FsPointOfUse).IsRequired().HasMaxLength(50);
            Property(u => u.SupplierName).IsRequired().HasMaxLength(100);
            Property(u => u.IsFsSupplier).IsRequired();
        }
    }
}
