using System;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// SAP工艺外协送出/送回
    /// 用于创建ERP相关订单内容
    /// </summary>
    public class SapCooperSendInput
    {
        /// <summary>
        /// SAP制造订单
        /// </summary>
        [Required]
        public string SapMOrderNumber { get; set; }

        /// <summary>
        /// SAP制造订单工序号（交接工序）
        /// </summary>
        [Required]
        public string SapMOrderProcessNumber { get; set; }

        /// <summary>
        /// 交接方向
        /// </summary>
        [Required]
        public SapCooperSendDirection Direction { get; set; }

        /// <summary>
        /// 交接数量
        /// </summary>
        [Required]
        public decimal HandOverQuantity { get; set; }

        public enum SapCooperSendDirection
        {
            /// <summary>
            /// 送出
            /// </summary>
            SendOut = 0,
            /// <summary>
            /// 送回
            /// </summary>
            SendBack = 1,
        }
    }
}
