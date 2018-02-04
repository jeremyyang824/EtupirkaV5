using Abp.Domain.Entities.Auditing;
using Etupirka.Domain.External.Entities.Dmes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Entities
{
    public class SapMOrderProcessDispatchPrepare : AuditedEntity
    {
        /// <summary>
        /// 派工单和工单的关联表ID
        /// </summary>
        public int DispatchWorKTicketID { get; set; }


        ///// <summary>
        ///// 订单号
        ///// </summary>
        //public string OrderNumber { get; set; }

        ///// <summary>
        ///// SAP生产订单工序
        ///// </summary>
        //public SapMOrderProcess SapMOrderProcess { get; set; }

        /// <summary>
        /// 工作中心ID
        /// </summary>
        public string WorkCenterID { get; set; }


        /// <summary>
        /// NC程序 已准备
        /// </summary>
        public short? NC_IsPreparedFinished { get; set; }

        ///// <summary>
        ///// NC程序 开始时间
        ///// </summary>
        //public DateTime? NC_StartDate { get; set; }

        /// <summary>
        /// NC程序 要求完工时间
        /// </summary>
        public DateTime? NC_RequiredDate { get; set; }

        ///// <summary>
        ///// NC程序 完工时间
        ///// </summary>
        //public DateTime? NC_FinishDate { get; set; }

        /// <summary>
        /// 刀具配刀 已准备
        /// </summary>
        public short? Tooling_IsPreparedFinished { get; set; }

        ///// <summary>
        ///// 刀具配刀 开始时间
        ///// </summary>
        //public DateTime? Tooling_StartDate { get; set; }

        /// <summary>
        /// 刀具配刀 要求完工时间
        /// </summary>
        public DateTime? Tooling_RequiredDate { get; set; }

        ///// <summary>
        ///// 刀具配刀 完工时间
        ///// </summary>
        //public DateTime? Tooling_FinishDate { get; set; }

        /// <summary>
        /// 模夹具 已准备
        /// </summary>
        public bool? Mould_IsPreparedFinished { get; set; }

        ///// <summary>
        ///// 模夹具 开始时间
        ///// </summary>
        //public DateTime? Mould_StartDate { get; set; }

        ///// <summary>
        ///// 模夹具 要求完工时间
        ///// </summary>
        //public DateTime? Mould_RequiredDate { get; set; }

        ///// <summary>
        ///// 模夹具 完工时间
        ///// </summary>
        //public DateTime? Mould_FinishDate { get; set; }

        /// <summary>
        /// 特殊工位器 已准备
        /// </summary>
        public bool? Special_IsPreparedFinished { get; set; }

        ///// <summary>
        ///// 特殊工位器 开始时间
        ///// </summary>
        //public DateTime? Special_StartDate { get; set; }

        ///// <summary>
        ///// 特殊工位器 要求完工时间
        ///// </summary>
        //public DateTime? Special_RequiredDate { get; set; }

        ///// <summary>
        ///// 特殊工位器 完工时间
        ///// </summary>
        //public DateTime? Special_FinishDate { get; set; }

        public virtual IList<SapMOrderProcessDispatchPrepareStep> PrepareSteps { get; set; }

    }
}
