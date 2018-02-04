using System;
using System.Collections.Generic;

namespace Etupirka.Domain.External.Entities.Bapi
{
    /// <summary>
    /// 获取订单结果
    /// </summary>
    public class GetSapOrdersOutput
    {
        /// <summary>
        /// 取得的订单总数
        /// </summary>
        public int TotalOrderCounts { get; private set; }

        /// <summary>
        /// 订单列表
        /// </summary>
        public IReadOnlyList<BapiOrderOutput> OrderList { get; set; }

        public GetSapOrdersOutput(int orderCount, List<BapiOrderOutput> orderList)
        {
            this.TotalOrderCounts = orderCount;
            this.OrderList = orderList;
        }
    }
}
