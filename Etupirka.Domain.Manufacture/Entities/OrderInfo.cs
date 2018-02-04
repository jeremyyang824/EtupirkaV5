using System;
using Abp.Domain.Values;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderInfo : ValueObject<OrderInfo>
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 订单行号
        /// (SAP生产订单无行号)
        /// </summary>
        public int? LineNumber { get; set; }

        /// <summary>
        /// 根据SAP订单创建订单信息
        /// </summary>
        /// <param name="sapOrder">SAP订单</param>
        /// <returns>订单信息</returns>
        public static OrderInfo CreateFromSap(SapMOrder sapOrder)
        {
            if (sapOrder == null)
                throw new ArgumentNullException("sapOrder");

            return new OrderInfo
            {
                SourceName = OrderSourceNames.SAP,
                OrderNumber = sapOrder.OrderNumber
            };
        }
    }
}
