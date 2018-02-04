using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// SAP工艺外协接口日志明细
    /// </summary>
    [AutoMapFrom(typeof(SapMOrderProcessCooperateStep))]
    public class SapCooperProcessLogStepOutput : AuditedEntityDto
    {
        /// <summary>
        /// SAP生产订单工序外协信息ID
        /// </summary>
        public int SapMOrderProcessCooperateId { get; set; }
        
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
