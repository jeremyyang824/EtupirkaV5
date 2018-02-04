using System;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class SapMOrderProcessCooperateConfiguration : EntityBaseConfiguration<SapMOrderProcessCooperate, int>
    {
        public SapMOrderProcessCooperateConfiguration()
        {
            ToTable("t_manu_SapMOrderProcessCooperate");

            Property(u => u.CooperateType).IsRequired();
            Property(u => u.CooperaterCode).IsRequired().HasMaxLength(50);
            Property(u => u.CooperaterName).IsRequired().HasMaxLength(200);
            Property(u => u.CooperaterPrice).IsRequired().HasPrecision(18, 4);
            Property(u => u.CooperaterFsPointOfUse).IsOptional().HasMaxLength(50);

            Property(u => u.IsPrepareFinished).IsOptional();
            Property(u => u.FsAuxiProcessCode).IsOptional().HasMaxLength(50);
            Property(u => u.FsWorkProcessCode).IsOptional().HasMaxLength(50);
            Property(u => u.HandOverQuantity).IsOptional().HasPrecision(18, 4);

            Property(u => u.IsSapPoRequestReleased).IsOptional();
            Property(u => u.SapPoRequestNumber).IsOptional().HasMaxLength(50);

            Property(u => u.IsSapPomtFinished).IsOptional();
            Property(u => u.SapPoNumber).IsOptional().HasMaxLength(50);
            Property(u => u.SapPoLine).IsOptional().HasMaxLength(50);

            Property(u => u.IsFsComtFinished).IsOptional();
            Property(u => u.FsCoNumber).IsOptional().HasMaxLength(50);

            Property(u => u.IsFsMomtFinished).IsOptional();
            Property(u => u.FsMoNumber).IsOptional().HasMaxLength(50);
            Property(u => u.IsFsPickFinished).IsOptional();
            Property(u => u.IsMesInspectFinished).IsOptional();
            Property(u => u.MesInspectQualified).IsOptional().HasPrecision(18, 4);
            Property(u => u.IsFsMorvFinished).IsOptional();
            Property(u => u.LotNumber).IsOptional().HasMaxLength(50);
            Property(u => u.IsFsImtrFinished).IsOptional();
            Property(u => u.ImtrDocumentNumberPrefix).IsOptional().HasMaxLength(50);
            Property(u => u.ImtrDocumentNumberSerialNumber).IsOptional();
            Property(u => u.IsFsImtrSalesFinished).IsOptional();
            Property(u => u.IsFsShipFinished).IsOptional();

            Property(u => u.IsSapPoReleased).IsOptional();

            Property(u => u.IsSapPorvFinished).IsOptional();

            //一对0..1
            HasRequired(s => s.SapMOrderProcess).WithMany().HasForeignKey(t => t.SapMOrderProcessId)
                .WillCascadeOnDelete(false);
        }
    }
}
