using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Vmes
{
    public class IsInspectedInput
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

        /// <summary>
        /// 工序号
        /// </summary>
        [Required]
        public string ProcessNumber { get; set; }
    }
}
