using System;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.SapMOrderManage.Dto
{
    /// <summary>
    /// SAP制造订单工序（含外协信息）
    /// </summary>
    [AutoMapFrom(typeof(SapMOrderProcess))]
    public class SapMOrderProcessWithCooperateOutput : SapMOrderProcessOutput
    {
        public int? ProcessCooperateId { get; set; }

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

        /// <summary>
        /// 东厂外协时的Fs使用点信息
        /// </summary>
        public string CooperaterFsPointOfUse { get; set; }


        /// <summary>
        /// FS销售订单号
        /// </summary>
        public string FsCoNumber { get; set; }

        /// <summary>
        /// FS生产订单号
        /// </summary>
        public string FsMoNumber { get; set; }

        /// <summary>
        /// MES质检完成
        /// </summary>
        public bool? IsMesInspectFinished { get; set; }

        /// <summary>
        /// MES质检合格数量
        /// </summary>
        public decimal? MesInspectQualified { get; set; }
    }
}
