using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 交接单
    /// </summary>
    [AutoMapFrom(typeof(HandOverBill))]
    public class HandOverBillOutput : CreationAuditedEntityDto
    {
        /// <summary>
        /// 交接单编号
        /// </summary>
        public string BillCode { get; set; }

        /// <summary>
        /// 转出部门
        /// </summary>
        public HandOverDepartmentDto TransferSource { get; set; }

        /// <summary>
        /// 转入类型
        /// </summary>
        public HandOverTargetType TransferTargetType { get; set; }

        /// <summary>
        /// 转入类型
        /// </summary>
        public string TransferTargetTypeName
        {
            get { return this.TransferTargetType.GetDescription(); }
        }

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

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatorUserName { get; set; }

        /// <summary>
        /// 交接单送出日期
        /// </summary>
        public DateTime? HandOverDate { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public HandOverBillState BillState { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string BillStateName
        {
            get { return this.BillState.GetDescription(); }
        }
    }
}
