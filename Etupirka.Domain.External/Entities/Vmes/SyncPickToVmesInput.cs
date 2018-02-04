using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Vmes
{
    /// <summary>
    /// 从FS同步PICK到可视化MES
    /// </summary>
    public class SyncPickToVmesInput
    {
        /// <summary>
        /// FS订单号
        /// </summary>
        [Required]
        public string MONumber { get; set; }

        /// <summary>
        /// FS行号
        /// </summary>
        [Required]
        public int MOLineNumber { get; set; }
    }
}
