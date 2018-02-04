using System;
using System.Collections.Generic;
using Abp.Domain.Values;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接供应商信息
    /// </summary>
    public class HandOverSupplier : ValueObject<HandOverSupplier>
    {
        /// <summary>
        /// 供方代码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供方名称
        /// </summary>
        public string SupplierName { get; set; }

        public void Clear()
        {
            this.SupplierCode = null;
            this.SupplierName = null;
        }
    }
}
