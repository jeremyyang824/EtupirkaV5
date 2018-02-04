using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    /// <summary>
    /// SAP工艺准备情况明细
    /// </summary>
    [AutoMapFrom(typeof(SapMOrderProcessDispatchPrepare))]
    public class DispatchOrderPrepareOutput : AuditedEntityDto
    {

        #region PrepareInfo

        /// <summary>
        /// 
        /// </summary>
        public int DispatchWorkTicketID { get; set; }


        /// <summary>
        /// NC程序 已准备
        /// </summary>
        public short? NC_IsPreparedFinished { get; set; }

        /// <summary>
        /// 刀具配刀 已准备
        /// </summary>
        public short? Tooling_IsPreparedFinished { get; set; }

        /// <summary>
        /// 模夹具 已准备
        /// </summary>
        public bool? Mould_IsPreparedFinished { get; set; }

        /// <summary>
        /// 特殊工位器 已准备
        /// </summary>
        public bool? Special_IsPreparedFinished { get; set; }

        public IList<DispatchOrderPrepareStepOutput> PrepareSteps { get; set; }

        #endregion

    }
}
