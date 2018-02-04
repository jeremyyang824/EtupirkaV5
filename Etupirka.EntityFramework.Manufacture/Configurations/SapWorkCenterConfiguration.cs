using System;
using System.Collections.Generic;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// SAP工作中心定义
    /// </summary>
    public class SapWorkCenterConfiguration : EntityBaseConfiguration<SapWorkCenter, Guid>
    {
        public SapWorkCenterConfiguration()
        {
            ToTable("t_manu_SapWorkCenter");

            Property(u => u.WorkCenterCode).IsRequired().HasMaxLength(50);
            Property(u => u.WorkCenterName).IsRequired().HasMaxLength(100);
            Property(u => u.ProductionPlant).IsRequired().HasMaxLength(50);
            Property(u => u.OBJTY).IsRequired().HasMaxLength(50);
            Property(u => u.OBJID).IsRequired().HasMaxLength(50);

            Property(u => u.PLANV).IsOptional().HasMaxLength(50);
            Property(u => u.VERWE).IsOptional().HasMaxLength(50);
            Property(u => u.XSPRR).IsOptional().HasMaxLength(50);
            Property(u => u.LVORM).IsOptional().HasMaxLength(50);
            Property(u => u.KAPAR).IsOptional().HasMaxLength(50);
            Property(u => u.MEINS).IsOptional().HasMaxLength(50);

            Property(u => u.AEDAT_GRND).IsOptional();
            Property(u => u.AENAM_GRND).IsOptional().HasMaxLength(50);
        }
    }
}
