using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 保存交接单输入内容
    /// </summary>
    [AutoMapTo(typeof(HandOverBill))]
    public class SaveHandOverBillInput : IShouldNormalize
    {
        /// <summary>
        /// 交接单ID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 转出部门
        /// </summary>
        public HandOverDepartmentDto TransferSource { get; set; }

        /// <summary>
        /// 转入类型
        /// </summary>
        [Required]
        public HandOverTargetType TransferTargetType { get; set; }

        /// <summary>
        /// 转入部门
        /// </summary>
        public HandOverDepartmentDto TransferTargetDepartment { get; set; }

        /// <summary>
        /// 转入供应商编码
        /// </summary>
        public HandOverSupplierDto TransferTargetSupplier { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public void Normalize()
        {
            if (this.TransferTargetDepartment == null)
                this.TransferTargetDepartment = new HandOverDepartmentDto();
            if (this.TransferTargetSupplier == null)
                this.TransferTargetSupplier = new HandOverSupplierDto();
        }
    }
}
