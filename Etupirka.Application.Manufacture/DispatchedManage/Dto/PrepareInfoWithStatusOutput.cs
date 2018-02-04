using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    public class PrepareInfoWithStatusOutput
    {
        /// <summary>
        /// 派工单和工单的关联表ID
        /// </summary>
        public int DispatchWorKTicketID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 道序号
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialDescription { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string ActualWorkID { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string ActualWorkName { get; set; }

        /// <summary>
        /// 齐备性 业务类型（SapMOrderProcessDispatchStepTransTypes）
        /// </summary>
        public string StepTransactionType { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public short? StepStatus { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public string StepStatusStr { get; set; }

        /// <summary>
        /// 接口执行反馈消息
        /// </summary>
        public string StepResultMessage { get; set; }

        /// <summary>
        /// 要求完工时间 默认是呼叫时间+1D
        /// </summary>
        public string StepRequiredDate { get; set; }

        /// <summary>
        /// 呼叫时间
        /// </summary>
        public string StepStartedDate { get; set; }

        /// <summary>
        /// 完工时间
        /// </summary>
        public string StepFinishedDate { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public string StepDelayed { get; set; }
    }
}
