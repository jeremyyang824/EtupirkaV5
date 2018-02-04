using System;

namespace Etupirka.Domain.External.Entities.Fsti
{
    public class ShipInput
    {
        public string CoNumber { get; set; }
        public int CoLineNumber { get; set; }
        public decimal ShippedQuantity { get; set; }
        public string Stockroom { get; set; }
        public string Bin { get; set; }
        public string InventoryCategory { get; set; }
        public string LotNumber { get; set; }
    }
}
