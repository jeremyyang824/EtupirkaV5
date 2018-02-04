using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// SAP工艺外协相关接口日志
    /// </summary>
    [AutoMapFrom(typeof(SapMOrderProcessCooperate))]
    public class SapCooperProcessLogOutput : AuditedEntityDto
    {
        #region 订单信息

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialDescription { get; set; }

        /// <summary>
        /// 订单总计数量
        /// </summary>
        public decimal TargetQuantity { get; set; }

        #endregion

        #region 道序信息

        /// <summary>
        /// 工序号(VORNR)
        /// </summary>
        public string OperationNumber { get; set; }

        /// <summary>
        /// 工作中心代码(ARBPL)
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心描述(KTEXT)
        /// </summary>
        public string WorkCenterName { get; set; }

        #endregion

        #region SapMOrderProcessCooperate

        /// <summary>
        /// 外协类型
        /// </summary>
        public SapMOrderProcessCooperateType CooperateType { get; set; }

        /// <summary>
        /// 使用点（东厂外协）/供方代码（供方外协）
        /// </summary>
        public string CooperaterCode { get; set; }

        /// <summary>
        /// 使用点名称（东厂外协）/供方名称（供方外协）
        /// </summary>
        public string CooperaterName { get; set; }

        /// <summary>
        /// 外协价格
        /// </summary>
        public decimal CooperaterPrice { get; set; }

        /// <summary>
        /// 准备完成
        /// </summary>
        public bool? IsPrepareFinished { get; set; }

        /// <summary>
        /// 交接数量
        /// </summary>
        public decimal? HandOverQuantity { get; set; }

        /// <summary>
        /// FS准备工艺代码
        /// </summary>
        public string FsAuxiProcessCode { get; set; }

        /// <summary>
        /// FS操作工艺代码
        /// </summary>
        public string FsWorkProcessCode { get; set; }

        /// <summary>
        /// SAP采购请求批准
        /// </summary>
        public bool? IsSapPoRequestReleased { get; set; }

        /// <summary>
        /// SAP采购申请号
        /// </summary>
        public string SapPoRequestNumber { get; set; }

        /// <summary>
        /// 1.SAP采购订单创建
        /// </summary>
        public bool? IsSapPomtFinished { get; set; }

        /// <summary>
        /// SAP采购订单号
        /// </summary>
        public string SapPoNumber { get; set; }

        /// <summary>
        /// SAP采购订单行号
        /// </summary>
        public string SapPoLine { get; set; }


        /// <summary>
        /// 2.FS销售订单创建
        /// </summary>
        public bool? IsFsComtFinished { get; set; }

        /// <summary>
        /// FS销售订单号
        /// </summary>
        public string FsCoNumber { get; set; }
        
        /// <summary>
        /// 3.FS生产订单创建
        /// </summary>
        public bool? IsFsMomtFinished { get; set; }

        /// <summary>
        /// FS生产订单号
        /// </summary>
        public string FsMoNumber { get; set; }
        
        /// <summary>
        /// 4.FS发料清单创建
        /// </summary>
        public bool? IsFsPickFinished { get; set; }

        /// <summary>
        /// MES质检完成
        /// </summary>
        public bool? IsMesInspectFinished { get; set; }

        /// <summary>
        /// MES质检合格数量
        /// </summary>
        public decimal? MesInspectQualified { get; set; }

        /// <summary>
        /// 5.FS入库送检
        /// </summary>
        public bool? IsFsMorvFinished { get; set; }

        /// <summary>
        /// Morv生成的LotNumber
        /// </summary>
        public string LotNumber { get; set; }

        /// <summary>
        /// 6.FS验收移库
        /// </summary>
        public bool? IsFsImtrFinished { get; set; }

        /// <summary>
        /// Imtr文档号
        /// “XC-YYMM”+3位流水号
        /// </summary>
        public string ImtrDocumentNumber { get; set; }

        /// <summary>
        /// 7.FS移销售库
        /// </summary>
        public bool? IsFsImtrSalesFinished { get; set; }

        /// <summary>
        /// 8.FS发运客户
        /// </summary>
        public bool? IsFsShipFinished { get; set; }

        /// <summary>
        /// SAP采购订单批准
        /// </summary>
        public bool? IsSapPoReleased { get; set; }

        /// <summary>
        /// 9.SAP采购订单入库
        /// </summary>
        public bool? IsSapPorvFinished { get; set; }

        #endregion

        public IList<SapCooperProcessLogStepOutput> CooperateSteps { get; set; }
    }
}
