using System;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class SapMOrderProcessCooperateStepConfiguration : EntityBaseConfiguration<SapMOrderProcessCooperateStep, int>
    {
        public SapMOrderProcessCooperateStepConfiguration()
        {
            ToTable("t_manu_SapMOrderProcessCooperateStep");

            Property(u => u.StepTransactionType).IsRequired().HasMaxLength(50);
            Property(u => u.StepName).IsRequired().HasMaxLength(100);
            Property(u => u.IsStepSuccess).IsRequired();
            Property(u => u.StepResultMessage).IsRequired().HasMaxLength(2000);

            //一对多
            HasRequired(u => u.CooperateInfo).WithMany(v => v.CooperateSteps).HasForeignKey(u => u.SapMOrderProcessCooperateId);
        }
    }
}
