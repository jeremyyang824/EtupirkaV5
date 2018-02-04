using System;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 订单信息
    /// </summary>
    [AutoMap(typeof(OrderInfo))]
    public class OrderInfoDto
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

    }
}
