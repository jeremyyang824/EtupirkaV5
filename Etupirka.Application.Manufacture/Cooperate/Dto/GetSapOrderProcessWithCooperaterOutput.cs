using System;
using System.Collections.Generic;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    public class GetSapOrderProcessWithCooperaterOutput
    {
        #region 订单信息

        public Guid OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// MRP控制者
        /// (100:制造订单; 200装配订单)
        /// </summary>
        public string MRPController { get; set; }

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

        /// <summary>
        /// WBS元素
        /// </summary>
        public string WBSElement { get; set; }

        #endregion

        #region 工艺信息

        public Guid ProcessId { get; set; }

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
        /// 工作中心代码(ARBPL)
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心描述(KTEXT)
        /// </summary>
        public string WorkCenterName { get; set; }

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

        #endregion

        #region 外协信息

        public int? CooperateId { get; set; }

        /// <summary>
        /// 外协类型
        /// </summary>
        public SapMOrderProcessCooperateType? CooperateType { get; set; }

        /// <summary>
        /// 外协类型
        /// </summary>
        public string CooperateTypeName => this.CooperateType?.GetDescription();

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

        #endregion
    }
}
