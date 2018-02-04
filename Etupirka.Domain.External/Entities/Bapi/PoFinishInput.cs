using System;

namespace Etupirka.Domain.External.Entities.Bapi
{
    /// <summary>
    /// 采购订单完工输入
    /// </summary>
    public class PoFinishInput
    {
        /// <summary>
        /// 物料号(道序外协类采购为物料号采购，料号可为空)
        /// </summary>
        public string MATERIAL { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string PLANT { get; set; }

        /// <summary>
        /// 库存地点，收货入库的库存地点
        /// </summary>
        public string STGE_LOC { get; set; }

        /// <summary>
        /// 批号，不提供时系统会自动创建
        /// </summary>
        public string BATCH { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string MOVE_TYPE { get; set; }

        /// <summary>
        /// 库存类型：
        /// STCK_TYPE非限制使用、2质检状态、3冻结
        /// </summary>
        public string STCK_TYPE { get; set; }
        
        /// <summary>
        /// 供应商帐户号
        /// </summary>
        public string VENDOR { get; set; }

        /// <summary>
        /// 客户帐户号
        /// </summary>
        public string CUSTOMER { get; set; }

        /// <summary>
        /// 以输入单位计的数量
        /// </summary>
        public decimal ENTRY_QNT { get; set; }

        /// <summary>
        /// 条目单位
        /// </summary>
        public string ENTRY_UOM { get; set; }

        /// <summary>
        /// PO_NUMBER
        /// </summary>
        public string PO_NUMBER { get; set; }

        /// <summary>
        /// 采购凭证的项目编号
        /// </summary>
        public string PO_ITEM { get; set; }

        /// <summary>
        /// 移动标识
        /// B 按采购订单的货物移动
        /// F 有关生产单的货物移动
        /// L 有关交货通知的货物移动
        /// K 看板需求的货物移动（WM－仅限内部）
        /// O '提供物料'"'消耗的后续调整
        /// W比例的后续调整/产品单位物料
        /// </summary>
        public string MVT_IND { get; set; }
    }
}
