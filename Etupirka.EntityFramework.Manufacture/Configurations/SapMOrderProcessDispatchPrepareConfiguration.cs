using Etupirka.Domain.Manufacture.Entities;
using Etupirka.EntityFramework.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    public class SapMOrderProcessDispatchPrepareConfiguration : EntityBaseConfiguration<SapMOrderProcessDispatchPrepare, int>
    {
        public SapMOrderProcessDispatchPrepareConfiguration()
        {
            ToTable("t_manu_SapMOrderProcessDispatchPrepare");

            Property(u => u.DispatchWorKTicketID).IsRequired();
            Property(u => u.WorkCenterID).IsOptional();

            Property(u => u.NC_IsPreparedFinished).IsOptional();
            //Property(u => u.NC_StartDate).IsOptional();
            //Property(u => u.NC_RequiredDate).IsOptional();
            //Property(u => u.NC_FinishDate).IsOptional();

            Property(u => u.Tooling_IsPreparedFinished).IsOptional();
            //Property(u => u.Tooling_StartDate).IsOptional();
            //Property(u => u.Tooling_RequiredDate).IsOptional();
            //Property(u => u.Tooling_FinishDate).IsOptional();

            Property(u => u.Mould_IsPreparedFinished).IsOptional();
            //Property(u => u.Mould_StartDate).IsOptional();
            //Property(u => u.Mould_RequiredDate).IsOptional();
            //Property(u => u.Mould_FinishDate).IsOptional();

            Property(u => u.Special_IsPreparedFinished).IsOptional();
            //Property(u => u.Special_StartDate).IsOptional();
            //Property(u => u.Special_RequiredDate).IsOptional();
            //Property(u => u.Special_FinishDate).IsOptional();

            //多对一

            //一对多

        }
    }
}
