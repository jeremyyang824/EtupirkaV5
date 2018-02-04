using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// SAP生产订单工序外协步骤
    /// </summary>
    public class SapMOrderProcessCooperateStep : AuditedEntity
    {
        /// <summary>
        /// SAP生产订单工序外协信息ID
        /// </summary>
        public int SapMOrderProcessCooperateId { get; set; }

        /// <summary>
        /// SAP生产订单工序外协信息
        /// </summary>
        public SapMOrderProcessCooperate CooperateInfo { get; set; }

        /// <summary>
        /// 业务类型（SapMOrderProcessCooperateStepTransTypes）
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

        /// <summary>
        /// 接口执行反馈消息
        /// </summary>
        public string StepResultMessage { get; set; }
    }
}
