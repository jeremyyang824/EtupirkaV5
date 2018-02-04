using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Entities
{
    public class SapMOrderProcessDispatchPrepareStep : AuditedEntity
    {
        public int SapMOrderProcessDispatchPrepareId { get; set; }

        public virtual SapMOrderProcessDispatchPrepare PrepareInfo { get; set; }

        /// <summary>
        /// 业务类型（SapMOrderProcessDispatchStepTransTypes）
        /// </summary>
        public string StepTransactionType { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsStepSuccess { get; set; }

        ///// <summary>
        ///// 是否执行成功
        ///// </summary>
        //public short? StepStatus { get; set; }

        /// <summary>
        /// 接口执行反馈消息
        /// </summary>
        public string StepResultMessage { get; set; }
        

        ///// <summary>
        ///// NC程序 开始时间
        ///// </summary>
        //public DateTime? StepStartDate { get; set; }

        ///// <summary>
        ///// NC程序 要求完工时间
        ///// </summary>
        //public DateTime? StepRequiredDate { get; set; }

        ///// <summary>
        ///// NC程序 完工时间
        ///// </summary>
        //public DateTime? StepFinishDate { get; set; }
    }
}
