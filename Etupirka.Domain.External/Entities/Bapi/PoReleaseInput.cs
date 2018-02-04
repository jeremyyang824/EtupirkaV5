using System;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Bapi
{
    public class PoReleaseInput
    {
        /// <summary>
        /// 采购申请号
        /// </summary>
        [Required]
        public string PoNumber { get; set; }

        /// <summary>
        /// 50  计划部领导
        /// </summary>
        [Required]
        public string RelCode { get; set; }
    }
}
