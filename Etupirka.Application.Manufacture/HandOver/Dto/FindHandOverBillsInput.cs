using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Runtime.Validation;
using Etupirka.Application.Portal.Dto;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 查找交接单
    /// </summary>
    public class FindHandOverBillsInput : PagedAndFilteredInput, IShouldNormalize
    {
        /// <summary>
        /// 交接单号（可空）
        /// </summary>
        public string BillCode { get; set; }

        /// <summary>
        /// 交接行订单号（可空）
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 交接行零件编码（可空）
        /// </summary>
        public string ItemNumber { get; set; }

        /// <summary>
        /// 转出部门名称
        /// </summary>
        public string TransferSourceName { get; set; }

        /// <summary>
        /// 转入部门/供应商名称(编码)
        /// </summary>
        public string TransferTargetName { get; set; }

        /// <summary>
        /// 查询开始日期
        /// </summary>
        public DateTime? RangeBegin { get; set; }

        /// <summary>
        /// 查询结束日期
        /// </summary>
        public DateTime? RangeEnd { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public HandOverBillState[] State { get; set; }

        public void Normalize()
        {
            this.BillCode = BillCode?.Trim();
            this.RangeBegin = RangeBegin?.DateBegin();
            this.RangeEnd = RangeEnd?.DateEnd();
            this.OrderNumber = OrderNumber?.Trim().ToUpper();
            this.ItemNumber = ItemNumber?.Trim().ToUpper();
        }
    }
}
