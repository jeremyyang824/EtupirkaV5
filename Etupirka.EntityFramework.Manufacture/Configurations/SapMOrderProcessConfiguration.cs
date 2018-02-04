using System;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// SAP生产订单工序
    /// </summary>
    public class SapMOrderProcessConfiguration : EntityBaseConfiguration<SapMOrderProcess, Guid>
    {
        public SapMOrderProcessConfiguration()
        {
            ToTable("t_manu_SapMOrderProcess");

            Property(u => u.RoutingNumber).IsRequired().HasMaxLength(50);
            Property(u => u.OrderCounter).IsRequired();
            Property(u => u.OperationNumber).IsRequired().HasMaxLength(50);
            Property(u => u.OperationCtrlCode).IsRequired().HasMaxLength(50);
            Property(u => u.ProductionPlant).IsRequired().HasMaxLength(50);

            Property(u => u.WorkCenterObjId).IsRequired().HasMaxLength(50);
            Property(u => u.WorkCenterCode).IsRequired().HasMaxLength(50);
            Property(u => u.WorkCenterName).IsRequired().HasMaxLength(100);

            Property(u => u.StandardText).IsRequired().HasMaxLength(100);
            Property(u => u.ProcessText1).IsRequired().HasMaxLength(500);
            Property(u => u.ProcessText2).IsRequired().HasMaxLength(500);

            Property(u => u.UMREN).IsRequired().HasPrecision(18, 4);
            Property(u => u.UMREZ).IsRequired().HasPrecision(18, 4);

            Property(u => u.Unit).IsRequired().HasMaxLength(50);
            Property(u => u.BaseQuantity).IsRequired().HasPrecision(18, 4);
            Property(u => u.ProcessQuantity).IsRequired().HasPrecision(18, 4);
            Property(u => u.ScrapQuantity).IsRequired().HasPrecision(18, 4);

            Property(u => u.VGE01).IsRequired().HasMaxLength(50);
            Property(u => u.VGW01).IsRequired().HasPrecision(18, 4);
            Property(u => u.VGE02).IsRequired().HasMaxLength(50);
            Property(u => u.VGW02).IsRequired().HasPrecision(18, 4);
            Property(u => u.VGE03).IsRequired().HasMaxLength(50);
            Property(u => u.VGW03).IsRequired().HasPrecision(18, 4);
            Property(u => u.VGE04).IsRequired().HasMaxLength(50);
            Property(u => u.VGW04).IsRequired().HasPrecision(18, 4);
            Property(u => u.VGE05).IsRequired().HasMaxLength(50);
            Property(u => u.VGW05).IsRequired().HasPrecision(18, 4);
            Property(u => u.VGE06).IsRequired().HasMaxLength(50);
            Property(u => u.VGW06).IsRequired().HasPrecision(18, 4);

            Property(u => u.ScheduleStartDate).IsOptional();
            Property(u => u.ScheduleFinishDate).IsOptional();

            Property(u => u.BANFN).IsOptional().HasMaxLength(50);
            Property(u => u.BNFPO).IsOptional().HasMaxLength(50);
            Property(u => u.LIFNR).IsOptional().HasMaxLength(50);

            //一对多
            HasRequired(u => u.SapMOrder).WithMany(v => v.OrderProcess).HasForeignKey(u => u.SapMOrderId)
                .WillCascadeOnDelete(false);
        }
    }
}
