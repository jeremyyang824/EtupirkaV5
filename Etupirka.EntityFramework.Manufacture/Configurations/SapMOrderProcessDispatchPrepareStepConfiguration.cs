using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class SapMOrderProcessDispatchPrepareStepConfiguration : EntityBaseConfiguration<SapMOrderProcessDispatchPrepareStep, int>
    {
        public SapMOrderProcessDispatchPrepareStepConfiguration()
        {
            ToTable("t_manu_SapMOrderProcessDispatchPrepareStep");

            Property(u => u.StepTransactionType).IsRequired().HasMaxLength(50);
            Property(u => u.StepName).IsRequired().HasMaxLength(100);
            Property(u => u.IsStepSuccess).IsRequired();
            //Property(u => u.StepStatus).IsOptional();
            Property(u => u.StepResultMessage).IsRequired().HasMaxLength(2000);
            //Property(u => u.StepStartDate).IsOptional();
            //Property(u => u.StepRequiredDate).IsOptional();
            //Property(u => u.StepFinishDate).IsOptional();

            //一对多
            HasRequired(u => u.PrepareInfo).WithMany(v => v.PrepareSteps).HasForeignKey(u => u.SapMOrderProcessDispatchPrepareId);
        }
    }
}
