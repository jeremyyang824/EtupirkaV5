using System;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 外协步骤业务类型
    /// </summary>
    public static class SapMOrderProcessCooperateStepTransTypes
    {
        /// <summary>
        /// 准备工作
        /// </summary>
        public const string ProcessPrepare = "准备";

        /// <summary>
        /// SAP采购请求批准
        /// </summary>
        public const string SapPoRequestRelease = "SapPoRequestRelease";

        /// <summary>
        /// 1.SAP采购订单创建
        /// </summary>
        public const string SapPomt = "SapPomt";

        /// <summary>
        /// 2.FS销售订单创建
        /// </summary>
        public const string FsComt = "FsComt";
        public const string FsComt_ADDHeader = "FsComt_ADDHeader";
        public const string FsComt_ADDLine = "FsComt_ADDLine";
        public const string FsComt_ADDLineText = "FsComt_ADDLineText";

        /// <summary>
        /// 3.FS生产订单创建
        /// </summary>
        public const string FsMomt = "FsMomt";
        public const string FsMomt_ADDHeader = "FsMomt_ADDHeader";
        public const string FsMomt_ADDLine = "FsMomt_ADDLine";
        public const string FsMomt_ADDLineText = "FsMomt_ADDLineText";

        /// <summary>
        /// 4.FS发料清单创建
        /// </summary>
        public const string FsPick = "FsPick";
        public const string FsPick_ADD_Auxi = "FsPick_ADD_Auxi";
        public const string FsPick_ADD_Work = "FsPick_ADD_Work";
        public const string FsPick_EDITDetail_Work = "FsPick_EDITDetail"; //操作工时需要修改量类为“I”（O与投产数量无关的，I与投产数量有关）
        public const string FsPick_SyncToMes = "FsPick_SyncToMes";  //将FS pick同步到可视化MES

        /// <summary>
        /// 5.FS入库送检
        /// MORVRByLot
        /// </summary>
        public const string FsMorv = "FsMorv";

        /// <summary>
        /// 6.FS验收移库
        /// IMTRByLot
        /// </summary>
        public const string FsImtr = "FsImtr";

        /// <summary>
        /// 7.FS移销售库
        /// IMTRByLot
        /// </summary>
        public const string FsImtrSales = "FsImtrSales";

        /// <summary>
        /// 8.FS发运客户
        /// SHIPIByLot
        /// </summary>
        public const string FsShip = "FsShip";

        /// <summary>
        /// SAP采购订单批准
        /// </summary>
        public const string SapPoRelease = "SapPoRelease";

        /// <summary>
        /// 8.SAP采购订单入库
        /// </summary>
        public const string SapPorv = "SapPorv";

        public const string Others = "其他";
    }
}
