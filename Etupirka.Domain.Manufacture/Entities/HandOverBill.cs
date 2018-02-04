using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单
    /// </summary>
    public class HandOverBill : CreationAuditedEntity
    {
        /// <summary>
        /// 交接单编号前缀
        /// yyyyMM
        /// </summary>
        public string BillCodePrefix { get; set; }

        /// <summary>
        /// 交接单编号流水号
        /// </summary>
        public string BillCodeSerialNumber { get; set; }

        /// <summary>
        /// 交接单编号
        /// 前缀+3位流水号
        /// </summary>
        public string BillCode
        {
            get { return BillCodePrefix + BillCodeSerialNumber; }
        }

        /// <summary>
        /// 转出部门
        /// </summary>
        public HandOverDepartment TransferSource { get; set; }

        /// <summary>
        /// 转入类型
        /// </summary>
        public HandOverTargetType TransferTargetType { get; set; }

        /// <summary>
        /// 转入部门
        /// TransferTargetType == HandOverTargetType.Department
        /// </summary>
        public HandOverDepartment TransferTargetDepartment { get; set; }

        /// <summary>
        /// 转入供应商编码
        /// TransferTargetType == HandOverTargetType.Supplier
        /// </summary>
        public HandOverSupplier TransferTargetSupplier { get; set; }

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
        /// 交接单行
        /// </summary>
        public virtual IList<HandOverBillLine> BillLines { get; set; }
    }
}
