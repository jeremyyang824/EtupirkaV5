using System;
using System.Collections.Generic;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// 取得SAP订单工序及外协信息
    /// </summary>
    public class GetSapOrderProcessWithCooperaterInput
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
