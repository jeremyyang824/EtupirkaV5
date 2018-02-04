using System;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// 更新外协信息
    /// </summary>
    public class UpdateSapOrderProcessCooperateInput
    {
        /// <summary>
        /// 外协记录ID
        /// </summary>
        [Required]
        public int CooperateId { get; set; }

        /// <summary>
        /// 外协类别
        /// </summary>
        [Required]
        public int CooperateType { get; set; }

        /// <summary>
        /// 外协类别
        /// </summary>
        [Required]
        public string CooperaterCode { get; set; }

        /// <summary>
        /// 外协类别
        /// </summary>
        [Required]
        public string CooperaterName { get; set; }

        /// <summary>
        /// 外协类别
        /// </summary>
        [Required]
        public decimal CooperaterPrice { get; set; }
    }
}
