using System;

namespace Etupirka.Domain.External.Entities.Fsti
{
    /// <summary>
    /// FSTI IMTR输入
    /// </summary>
    public class ImtrInput
    {
        public string ItemNumber { get; set; }
        public string StockroomFrom { get; set; }
        public string BinFrom { get; set; }
        public string InventoryCategoryFrom { get; set; }
        public decimal InventoryQuantity { get; set; }
        public string StockroomTo { get; set; }
        public string BinTo { get; set; }
        public string InventoryCategoryTo { get; set; }
        public string LotNumber { get; set; }
        public string OrderType { get; set; }
        public string OrderNumber { get; set; }
        public int LineNumber { get; set; }
        public string DocumentNumber { get; set; }
    }
}
