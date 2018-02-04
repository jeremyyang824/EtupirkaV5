using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// 交接供应商配置
    /// </summary>
    public class HandOverSupplierConfiguration : ComplexTypeConfiguration<HandOverSupplier>
    {
        public HandOverSupplierConfiguration()
        {
            Property(t => t.SupplierCode).IsOptional().HasMaxLength(20);
            Property(t => t.SupplierName).IsOptional().HasMaxLength(50);
        }
    }
}
