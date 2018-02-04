using System;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// SAP工艺外协质检完成
    /// </summary>
    public class SapCooperInspectedInput
    {
        /// <summary>
        /// FS制造订单
        /// （由SAP工艺外协自动创建）
        /// </summary>
        [Required]
        public string FsMOrderNumber { get; set; }

        /// <summary>
        /// 质检合格数量
        /// </summary>
        [Required]
        public decimal InspectQualified { get; set; }
    }
}
