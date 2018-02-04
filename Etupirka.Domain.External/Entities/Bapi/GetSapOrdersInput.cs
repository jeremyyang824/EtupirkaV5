using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Bapi
{
    /// <summary>
    /// 获取SAP订单输入
    /// </summary>
    public class GetSapOrdersInput
    {
        /// <summary>
        /// 工厂代码
        /// </summary>
        [Required]
        public string Plant { get; set; }

        /// <summary>
        /// 订单范围（开始）
        /// </summary>
        public string OrderNumberRangeBegin { get; set; }

        /// <summary>
        /// 订单范围（结束）
        /// </summary>
        public string OrderNumberRangeEnd { get; set; }

        /// <summary>
        /// 不限订单开始范围
        /// </summary>
        public bool IsNoneOrderNumberRangeBegin => string.IsNullOrWhiteSpace(this.OrderNumberRangeBegin);

        /// <summary>
        /// 不限订单结束范围
        /// </summary>
        public bool IsNoneOrderNumberRangeEnd => string.IsNullOrWhiteSpace(this.OrderNumberRangeEnd);
    }
}
