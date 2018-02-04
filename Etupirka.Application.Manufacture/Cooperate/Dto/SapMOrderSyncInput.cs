using System;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// SAP生产订单同步输入
    /// </summary>
    public class SapMOrderSyncInput
    {
        /// <summary>
        /// 订单范围（开始）
        /// </summary>
        public string OrderNumberRangeBegin { get; set; }

        /// <summary>
        /// 订单范围（结束）
        /// </summary>
        public string OrderNumberRangeEnd { get; set; }
    }
}
