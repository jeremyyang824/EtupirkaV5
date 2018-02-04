using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Entities
{
    public class SapMOrderProcessDispatchPrepareKey
    {
        /// <summary>
        /// 派工单和工单的关联表ID
        /// </summary>
        public int DispatchWorKTicketID { get; set; }

        /// <summary>
        /// 派工单和工单的关联表ID
        /// </summary>
        public short? StepStatus { get; set; }

        ///// <summary>
        ///// 订单号
        ///// </summary>
        //public string OrderNumber { get; set; }

        ///// <summary>
        ///// 物料编码
        ///// </summary>
        //public string MaterialNumber { get; set; }

        ///// <summary>
        ///// 物料名称
        ///// </summary>
        //public string MaterialDescription { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string RoutingNumber { get; set; }

        /// <summary>
        /// 齐备性 类型
        /// </summary>
        public string StepType { get; set; }
    }
}
