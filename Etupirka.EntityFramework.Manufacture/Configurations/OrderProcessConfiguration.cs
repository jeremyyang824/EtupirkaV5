using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// 工序信息
    /// </summary>
    public class OrderProcessConfiguration : ComplexTypeConfiguration<OrderProcess>
    {
        public OrderProcessConfiguration()
        {
            Property(t => t.ProcessNumber).IsRequired().HasMaxLength(20);
            Property(t => t.ProcessCode).IsRequired().HasMaxLength(50);
            Property(t => t.ProcessName).IsRequired().HasMaxLength(50);
            Property(t => t.PointOfUseId).IsOptional().HasMaxLength(50);
            Property(t => t.PointOfUseName).IsOptional().HasMaxLength(50);
        }
    }
}
