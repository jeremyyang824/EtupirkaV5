using System;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 交接供应商信息
    /// </summary>
    [AutoMap(typeof(HandOverSupplier))]
    public class HandOverSupplierDto
    {
        /// <summary>
        /// 供方代码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供方名称
        /// </summary>
        public string SupplierName { get; set; }

        public string SupplierFullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SupplierCode))
                    return $"[{SupplierCode}]{SupplierName}";
                return null;
            }
        }
    }
}
