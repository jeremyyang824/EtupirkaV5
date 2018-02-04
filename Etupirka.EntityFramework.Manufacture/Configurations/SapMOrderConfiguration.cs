using System;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// SAP生产订单信息配置
    /// </summary>
    public class SapMOrderConfiguration : EntityBaseConfiguration<SapMOrder, Guid>
    {
        public SapMOrderConfiguration()
        {
            ToTable("t_manu_SapMOrder");

            Property(u => u.OrderNumber).IsRequired().HasMaxLength(50);
            Property(u => u.ProductionPlant).IsRequired().HasMaxLength(50);
            Property(u => u.MRPController).IsRequired().HasMaxLength(50);
            Property(u => u.ProductionScheduler).IsRequired().HasMaxLength(50);
            Property(u => u.MaterialNumber).IsRequired().HasMaxLength(100);
            Property(u => u.MaterialDescription).IsRequired().HasMaxLength(200);

            Property(u => u.MaterialExternal).IsOptional().HasMaxLength(500);
            Property(u => u.MaterialGuid).IsOptional().HasMaxLength(50);
            Property(u => u.MaterialVersion).IsOptional().HasMaxLength(50);
            Property(u => u.RoutingNumber).IsOptional().HasMaxLength(50);

            Property(u => u.ScheduleReleaseDate).IsOptional();
            Property(u => u.ActualReleaseDate).IsOptional();
            Property(u => u.StartDate).IsOptional();
            Property(u => u.FinishDate).IsOptional();
            Property(u => u.ProductionStartDate).IsOptional();
            Property(u => u.ProductionFinishDate).IsOptional();
            Property(u => u.ActualStartDate).IsOptional();
            Property(u => u.ActualFinishDate).IsOptional();

            Property(u => u.TargetQuantity).IsRequired().HasPrecision(18, 4);
            Property(u => u.ScrapQuantity).IsRequired().HasPrecision(18, 4);
            Property(u => u.ConfirmedQuantity).IsRequired().HasPrecision(18, 4);
            Property(u => u.Unit).IsRequired().HasMaxLength(50);
            Property(u => u.UnitISO).IsOptional().HasMaxLength(50);

            Property(u => u.Priority).IsOptional().HasMaxLength(50);
            Property(u => u.OrderType).IsOptional().HasMaxLength(50);
            Property(u => u.WBSElement).IsOptional().HasMaxLength(50);
            Property(u => u.SystemStatus).IsOptional().HasMaxLength(100);

            Property(u => u.Batch).IsOptional().HasMaxLength(50);
            Property(u => u.ABLAD).IsOptional().HasMaxLength(50);
            Property(u => u.WEMPF).IsOptional().HasMaxLength(50);
            Property(u => u.AufkAenam).IsOptional().HasMaxLength(50);
            Property(u => u.AufkAedat).IsOptional();
            Property(u => u.AufkPhas0).IsOptional().HasMaxLength(20);
            Property(u => u.AufkPhas1).IsOptional().HasMaxLength(20);
            Property(u => u.AufkPhas2).IsOptional().HasMaxLength(20);
            Property(u => u.AufkPhas3).IsOptional().HasMaxLength(20);

            Property(u => u.SapId).IsRequired();
        }
    }
}
