using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 交接单行
    /// </summary>
    [AutoMapFrom(typeof(HandOverBillLine))]
    public class HandOverBillLineOutput : EntityDto
    {
        /// <summary>
        /// 交接单ID
        /// </summary>
        public int HandOverBillId { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfoDto OrderInfo { get; set; }

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
        public OrderProcessDto CurrentProcess { get; set; }

        /// <summary>
        /// 下个工序
        /// </summary>
        public OrderProcessDto NextProcess { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交接单行状态
        /// </summary>
        public HandOverBillLineState LineState { get; set; }

        /// <summary>
        /// 交接单行状态
        /// </summary>
        public string LineStateName
        {
            get { return this.LineState.GetDescription(); }
        }

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
        /// 交接单行状态
        /// </summary>
        public string InspectStateName
        {
            get { return this.InspectState.GetDescription(); }
        }

        /// <summary>
        /// 质检状态获取异常
        /// </summary>
        public string InspectStateErrorMessage { get; set; }
    }
}
