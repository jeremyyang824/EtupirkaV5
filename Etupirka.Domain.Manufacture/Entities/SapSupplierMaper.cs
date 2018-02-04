using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Entities;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// SAP供应商与FS使用点映射关系
    /// </summary>
    public class SapSupplierMaper : Entity
    {
        /// <summary>
        /// SAP供应商代码
        /// </summary>
        public string SapSupplierCode { get; set; }

        /// <summary>
        /// FS使用点
        /// </summary>
        public string FsPointOfUse { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 是否属于东厂区（东厂区供应商在流程中会创建FS订单信息）
        /// </summary>
        public bool IsFsSupplier { get; set; }

        /// <summary>
        /// 代表一个未定义映射的供应商
        /// </summary>
        public static SapSupplierMaper Empty { get; } = new SapSupplierMaper
        {
            SapSupplierCode = "",
            FsPointOfUse = "S",
            SupplierName = "",
            IsFsSupplier = false
        };

        public bool IsEmpty()
        {
            return this.SapSupplierCode == "";
        }
    }
}
