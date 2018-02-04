using System;

namespace Etupirka.Domain.External.Entities.Bapi
{
    /// <summary>
    /// 采购订单创建输入
    /// </summary>
    public class PoCreateInput
    {
        /// <summary>
        /// 采购凭证类型 
        /// DB	虚拟采购订单
        /// ENB	标准 PO DFPS
        /// EUB	库存转储订单
        /// FO	框架订单
        /// NB	标准采购订单
        /// UB	库存转储订单
        /// ZFY	费用类采购订单
        /// </summary>
        public string BSART { get; set; }

        /// <summary>
        /// 供应商或债权人的帐号
        /// </summary>
        public string LIFNR { get; set; }

        /// <summary>
        /// 采购组织
        /// </summary>
        public string EKORG { get; set; }

        /// <summary>
        /// 采购组
        /// </summary>
        public string EKGRP { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        public string BUKRS { get; set; }

        /// <summary>
        /// 您的参考
        /// </summary>
        public string IHREZ { get; set; }

        /// <summary>
        /// 采购凭证的项目编号
        /// </summary>
        public string EBELP { get; set; }

        /// <summary>
        /// 科目分配类别
        /// A 资产
        /// F 生产订单
        /// C 销售订单
        /// K 成本中心
        /// Q 项目 生成订单
        /// </summary>
        public string KNTTP { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 物料短文本
        /// </summary>
        public string TXZ01 { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal MENGE { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? EEIND { get; set; }

        /// <summary>
        /// 净价
        /// </summary>
        public decimal NETPR { get; set; }

        /// <summary>
        /// 货比码
        /// </summary>
        public string WAERS { get; set; }

        /// <summary>
        /// 物料组
        /// </summary>
        public string MATKL { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 需求跟踪号
        /// </summary>
        public string BEDNR { get; set; }

        /// <summary>
        /// 需求者/请求者姓名
        /// </summary>
        public string AFNAM { get; set; }

        /// <summary>
        /// 销售税代码
        /// </summary>
        public string MWSKZ { get; set; }

        /// <summary>
        /// 总帐科目/成本要素
        /// </summary>
        public string SAKTO { get; set; }

        /// <summary>
        /// 成本中心
        /// </summary>
        public string KOSTL { get; set; }

        /// <summary>
        /// 主资产号
        /// </summary>
        public string ANLN1 { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 行文本1
        /// </summary>
        public string STR1 { get; set; }

        /// <summary>
        /// 行文本2
        /// </summary>
        public string STR2 { get; set; }

        /// <summary>
        /// 行文本3
        /// </summary>
        public string STR3 { get; set; }

        /// <summary>
        /// 确认控制代码
        /// </summary>
        public string BSTAE { get; set; }

        /// <summary>
        /// 采购申请编号
        /// </summary>
        public string PREQNO { get; set; }

        /// <summary>
        /// 采购申请的项目编号
        /// </summary>
        public string PREQITEM { get; set; }

        /// <summary>
        /// 采购凭证中的项目类别
        /// </summary>
        public string EPSTP { get; set; }

        /// <summary>
        /// WBS元素
        /// </summary>
        public string WbsElement { get; set; }
    }
}
