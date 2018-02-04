using System;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Bapi
{
    public class PoRequestReleaseInput
    {
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Required]
        public string RequestNumber { get; set; }

        /// <summary>
        /// 50  计划部领导
        /// 51  综合计划部审批
        /// 52  设备动力部审批
        /// 53  行政事业部审批
        /// 54  安全保卫部审批
        /// 55  生产管理部审批
        /// 56  财务会计部审批
        /// </summary>
        [Required]
        public string RelCode { get; set; }
    }
}
