using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 交接单（含交接行状态统计）
    /// </summary>
    [AutoMapFrom(typeof(HandOverBill))]
    public class HandOverBillWithLineStatisticsOutput : HandOverBillOutput
    {
        /// <summary>
        /// 交接行总数
        /// </summary>
        public int TotalLineCount { get; set; }

        /// <summary>
        /// 待处理行数
        /// </summary>
        public int PendingLineCount { get; set; }

        /// <summary>
        /// 已接收行数
        /// </summary>
        public int ReceivedLineCount { get; set; }

        /// <summary>
        /// 已退回行数
        /// </summary>
        public int RejectedLineCount { get; set; }

        /// <summary>
        /// 已转送行数
        /// </summary>
        public int TransferLineCount { get; set; }
    }
}
