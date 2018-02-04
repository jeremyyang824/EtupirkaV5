using System;
using System.Collections.Generic;
using System.Linq;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class HandOverBillConfiguration : EntityBaseConfiguration<HandOverBill, int>
    {
        public HandOverBillConfiguration()
        {
            ToTable("t_manu_HandOverBill");

            Property(u => u.BillCodePrefix).IsRequired().HasMaxLength(20);
            Property(u => u.BillCodeSerialNumber).IsRequired().HasMaxLength(20);
            Property(u => u.TransferTargetType).IsRequired();
            Property(u => u.Remark).IsRequired().HasMaxLength(500);
            Property(u => u.CreatorUserName).IsRequired().HasMaxLength(20);
            Property(u => u.HandOverDate).IsOptional();
            Property(u => u.BillState).IsRequired();

            //多对一

            //一对多

        }
    }
}
