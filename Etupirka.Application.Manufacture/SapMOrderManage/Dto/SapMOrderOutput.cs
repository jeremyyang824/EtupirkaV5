using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.SapMOrderManage.Dto
{
    /// <summary>
    /// SAP制造订单
    /// </summary>
    [AutoMapFrom(typeof(SapMOrder))]
    public class SapMOrderOutput : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 生产工厂(WERKS)
        /// </summary>
        public string ProductionPlant { get; set; }

        /// <summary>
        /// MRP控制者
        /// (100:制造订单; 200装配订单)
        /// </summary>
        public string MRPController { get; set; }

        /// <summary>
        /// 生产调度员
        /// </summary>
        public string ProductionScheduler { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialDescription { get; set; }

        /// <summary>
        /// 物料描述长文本
        /// </summary>
        public string MaterialExternal { get; set; }

        /// <summary>
        /// 物料GUID
        /// </summary>
        public string MaterialGuid { get; set; }

        /// <summary>
        /// 物料版本编号
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 工艺路线号
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// 计划下达日期
        /// </summary>
        public DateTime? ScheduleReleaseDate { get; set; }

        /// <summary>
        /// 实际下达日期
        /// </summary>
        public DateTime? ActualReleaseDate { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 排产开始日期
        /// </summary>
        public DateTime? ProductionStartDate { get; set; }

        /// <summary>
        /// 计划完工日期
        /// </summary>
        public DateTime? ProductionFinishDate { get; set; }

        /// <summary>
        /// 实际开始日期
        /// </summary>
        public DateTime? ActualStartDate { get; set; }

        /// <summary>
        /// 实际结束日期
        /// </summary>
        public DateTime? ActualFinishDate { get; set; }

        /// <summary>
        /// 订单总计数量
        /// </summary>
        public decimal TargetQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        /// <summary>
        /// 根据 ATP 核查组件确认的订单数量
        /// </summary>
        public decimal ConfirmedQuantity { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitISO { get; set; }

        /// <summary>
        /// 订单优先级
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// WBS元素
        /// </summary>
        public string WBSElement { get; set; }

        /// <summary>
        /// 系统状态
        /// </summary>
        public string SystemStatus { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 卸货点
        /// </summary>
        public string ABLAD { get; set; }

        /// <summary>
        /// 收货方
        /// </summary>
        public string WEMPF { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string AufkAenam { get; set; }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime? AufkAedat { get; set; }

        /// <summary>
        /// 状态 "订单建立"
        /// </summary>
        public string AufkPhas0 { get; set; }

        /// <summary>
        /// 状态 "订单启用"
        /// </summary>
        public string AufkPhas1 { get; set; }

        /// <summary>
        /// 状态 "订单完成"
        /// </summary>
        public string AufkPhas2 { get; set; }

        /// <summary>
        /// 状态 "订单关闭"
        /// </summary>
        public string AufkPhas3 { get; set; }
    }
}
