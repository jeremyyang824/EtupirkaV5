using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 添加SAP交接单行
    /// </summary>
    public class AddSapHandOverBillLineInput
    {
        /// <summary>
        /// 交接单ID
        /// </summary>
        [Required]
        public int BillId { get; set; }

        /// <summary>
        /// 交接订单号
        /// </summary>
        [Required]
        public Guid SapMOrderId { get; set; }

        /// <summary>
        /// 交接道序号（首道序交接时为null）
        /// </summary>
        public Guid? SapMOrderProcessId { get; set; }

        /// <summary>
        /// 交接数量
        /// </summary>
        [Required]
        public decimal HandOverQuantity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
