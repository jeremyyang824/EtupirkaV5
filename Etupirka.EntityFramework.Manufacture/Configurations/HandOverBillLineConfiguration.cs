using System;
using System.Collections.Generic;
using System.Linq;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// 交接单行配置
    /// </summary>
    public class HandOverBillLineConfiguration : EntityBaseConfiguration<HandOverBillLine, int>
    {
        public HandOverBillLineConfiguration()
        {
            ToTable("t_manu_HandOverBillLine");

            Property(u => u.ItemNumber).IsRequired().HasMaxLength(50);
            Property(u => u.DrawingNumber).IsRequired().HasMaxLength(50);
            Property(u => u.ItemDescription).IsRequired().HasMaxLength(50);
            Property(u => u.HandOverQuantity).IsRequired().HasPrecision(18, 2);
            Property(u => u.Remark).IsRequired().HasMaxLength(500);
            Property(u => u.LineState).IsRequired();

            Property(u => u.OperatorUserId).IsOptional();
            Property(u => u.OperatorUserName).IsOptional().HasMaxLength(20);
            Property(u => u.OperatorDate).IsOptional();

            Property(u => u.InspectState).IsRequired();
            Property(u => u.InspectStateErrorMessage).IsOptional().HasMaxLength(500);

            //一对多
            HasRequired(u => u.HandOverBill).WithMany(v => v.BillLines).HasForeignKey(u => u.HandOverBillId);
        }
    }
}
