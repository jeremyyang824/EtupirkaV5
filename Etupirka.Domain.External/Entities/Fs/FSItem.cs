using System;

namespace Etupirka.Domain.External.Entities.Fs
{
    /// <summary>
    /// 物料主文件信息
    /// </summary>
    public class FSItem
    {
        /// <summary>
        /// 物料ERP ID
        /// </summary>
        public int ItemKey { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUM { get; set; }

        /// <summary>
        /// 购置类型
        /// </summary>
        public string MakeBuyCode { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// 所属产品
        /// </summary>
        public string FamilySubgroup { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNumber { get; set; }

        /// <summary>
        /// 优选库
        /// </summary>
        public string PreferredStockroom { get; set; }

        /// <summary>
        /// 优选位
        /// </summary>
        public string PreferredBin { get; set; }
    }
}
