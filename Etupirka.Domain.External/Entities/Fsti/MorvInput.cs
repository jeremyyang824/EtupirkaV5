using System;

namespace Etupirka.Domain.External.Entities.Fsti
{
    /// <summary>
    /// FSTI MORV输入
    /// </summary>
    public class MorvInput
    {
        public string MoNumber { get; set; }
        public int MoLineNumber { get; set; }
        public string ItemNumber { get; set; }
        public decimal ReceiptQuantity { get; set; }
        public string StockRoom { get; set; }
        public string Bin { get; set; }
        public string InventoryCategory { get; set; }
        public string LotNumber { get; set; }
    }
}
