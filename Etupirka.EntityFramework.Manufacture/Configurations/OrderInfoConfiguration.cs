using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderInfoConfiguration : ComplexTypeConfiguration<OrderInfo>
    {
        public OrderInfoConfiguration()
        {
            Property(t => t.SourceName).IsRequired().HasMaxLength(20);
            Property(t => t.OrderNumber).IsRequired().HasMaxLength(50);
            Property(t => t.LineNumber).IsOptional();
        }
    }
}
