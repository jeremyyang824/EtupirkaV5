using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Entities;
using Etupirka.Domain.Manufacture.Services;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单行
    /// </summary>
    public class HandOverBillLine : Entity
    {
        /// <summary>
        /// 交接单ID
        /// </summary>
        public int HandOverBillId { get; set; }

        /// <summary>
        /// 交接单
        /// </summary>
        public virtual HandOverBill HandOverBill { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemNumber { get; set; }

        /// <summary>
        /// 物料图号
        /// </summary>
        public string DrawingNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// 交接数量
        /// </summary>
        public decimal HandOverQuantity { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public OrderProcess CurrentProcess { get; set; }

        /// <summary>
        /// 下个工序
        /// </summary>
        public OrderProcess NextProcess { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交接单行状态
        /// </summary>
        public HandOverBillLineState LineState { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        public long? OperatorUserId { get; set; }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string OperatorUserName { get; set; }

        /// <summary>
        /// 处理日期
        /// </summary>
        public DateTime? OperatorDate { get; set; }

        /// <summary>
        /// 质检状态
        /// </summary>
        public HandOverBillLineInspectState InspectState { get; set; }

        /// <summary>
        /// 质检状态获取异常
        /// </summary>
        public string InspectStateErrorMessage { get; set; }

        /// <summary>
        /// 是否西厂区送出的SAP订单（非退回东厂）
        /// 订单类型为SAP 且 转出部门为“数字化制造基地（筹）”
        /// 且当前道序为西厂自加工（下个道序为外协东厂）
        /// </summary>
        public bool IsSapSendOut()
        {
            if (OrderInfo.SourceName == OrderSourceNames.SAP
                /*&& HandOverBill?.TransferSource?.OrganizationUnitCode == "35"*/
                /*&& CurrentProcess.PointOfUseId == null*/)
                return true;
            return false;
        }

        /// <summary>
        /// 是否东厂区送回的SAP订单（非退回西厂）
        /// 订单类型为SAP 且 转出部门非“数字化制造基地（筹）”
        /// 且当前道序为外协东厂（下个道序为西厂自加工）
        /// </summary>
        /// <returns></returns>
        public bool IsSapSendBack()
        {
            if (OrderInfo.SourceName == OrderSourceNames.SAP
                && HandOverBill?.TransferSource?.OrganizationUnitCode != "35"
                && CurrentProcess.PointOfUseId != null && HandOverSourceManager.StmcEastFsPointCode.Contains(CurrentProcess.PointOfUseId))
                return true;
            return false;
        }

    }
}
