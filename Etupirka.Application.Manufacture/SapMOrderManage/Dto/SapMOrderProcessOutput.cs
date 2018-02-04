﻿using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.SapMOrderManage.Dto
{
    /// <summary>
    /// SAP制造订单工序
    /// </summary>
    [AutoMapFrom(typeof(SapMOrderProcess))]
    public class SapMOrderProcessOutput : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// SAP生产订单Id
        /// </summary>
        public Guid SapMOrderId { get; set; }

        /// <summary>
        /// 工艺路线号(AUFPL)
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// 订单的通用计数器(APLZL)
        /// </summary>
        public int OrderCounter { get; set; }

        /// <summary>
        /// 工序号(VORNR)
        /// </summary>
        public string OperationNumber { get; set; }

        /// <summary>
        /// 控制码(STEUS)
        /// PP01:自加工不需检验; PP02:外协不需检验; PP06:文本(可外协); ZQ01:自加工需检验; ZQ02:外协需检验;
        /// </summary>
        public string OperationCtrlCode { get; set; }

        /// <summary>
        /// 生产工厂(WERKS)
        /// </summary>
        public string ProductionPlant { get; set; }


        /// <summary>
        /// 工作中心SAP系统ID(ARBID)
        /// </summary>
        public string WorkCenterObjId { get; set; }

        /// <summary>
        /// 工作中心代码(ARBPL)
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心描述(KTEXT)
        /// </summary>
        public string WorkCenterName { get; set; }


        /// <summary>
        /// 标准文本码（KTSCH）
        /// </summary>
        public string StandardText { get; set; }

        /// <summary>
        /// 工序短文本1(LTXA1)
        /// </summary>
        public string ProcessText1 { get; set; }

        /// <summary>
        /// 工序短文本2(LTXA2)
        /// </summary>
        public string ProcessText2 { get; set; }

        /// <summary>
        /// 用于转换工艺路线和工序单位的分母
        /// </summary>
        public decimal UMREN { get; set; }

        /// <summary>
        /// 用于转换任务清单和工序计量单位的计数器
        /// </summary>
        public decimal UMREZ { get; set; }

        /// <summary>
        /// 作业/工序的计量单位(MEINH)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 基本数量(BMSCH)
        /// </summary>
        public decimal BaseQuantity { get; set; }

        /// <summary>
        /// 工序数量(MGVRG)
        /// </summary>
        public decimal ProcessQuantity { get; set; }

        /// <summary>
        /// 工序废品(ASVRG)
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        #region 定额工时

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE01 { get; set; }

        /// <summary>
        /// 标准值（准备工时）
        /// </summary>
        public decimal VGW01 { get; set; }

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE02 { get; set; }

        /// <summary>
        /// 标准值（机器工时）
        /// </summary>
        public decimal VGW02 { get; set; }

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE03 { get; set; }

        /// <summary>
        /// 标准值（人工工时）
        /// </summary>
        public decimal VGW03 { get; set; }

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE04 { get; set; }

        /// <summary>
        /// 标准值（计划周期）
        /// </summary>
        public decimal VGW04 { get; set; }

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE05 { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public decimal VGW05 { get; set; }

        /// <summary>
        /// 标准值计量单位
        /// </summary>
        public string VGE06 { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public decimal VGW06 { get; set; }

        #endregion

        /// <summary>
        /// 最迟计划开始执行日期(SSAVD)
        /// </summary>
        public DateTime? ScheduleStartDate { get; set; }

        /// <summary>
        /// 最迟计划完成执行日期(SSEDD)
        /// </summary>
        public DateTime? ScheduleFinishDate { get; set; }
    }
}
