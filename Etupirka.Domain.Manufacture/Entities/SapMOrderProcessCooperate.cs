using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// SAP生产订单工序外协信息
    /// </summary>
    public class SapMOrderProcessCooperate : AuditedEntity
    {
        /// <summary>
        /// SAP生产订单工序ID
        /// </summary>
        public Guid SapMOrderProcessId { get; set; }

        /// <summary>
        /// SAP生产订单工序
        /// </summary>
        public SapMOrderProcess SapMOrderProcess { get; set; }


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
        /// 东厂外协时的Fs使用点信息
        /// </summary>
        public string CooperaterFsPointOfUse { get; set; }


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
        /// Imtr文档号前缀
        /// XC-YYMM
        /// </summary>
        public string ImtrDocumentNumberPrefix { get; set; }

        /// <summary>
        /// Imtr文档号流水号
        /// 3位流水号
        /// </summary>
        public int? ImtrDocumentNumberSerialNumber { get; set; }

        /// <summary>
        /// Imtr文档号
        /// “XC-YYMM”+3位流水号
        /// </summary>
        public string ImtrDocumentNumber
        {
            get
            {
                if (this.ImtrDocumentNumberPrefix == null || this.ImtrDocumentNumberSerialNumber == null)
                    return null;
                return $"{ImtrDocumentNumberPrefix}{ImtrDocumentNumberSerialNumber.Value:D3}";
            }
        }


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

        /// <summary>
        /// 步骤明细
        /// </summary>
        public virtual IList<SapMOrderProcessCooperateStep> CooperateSteps { get; set; }
    }
}
