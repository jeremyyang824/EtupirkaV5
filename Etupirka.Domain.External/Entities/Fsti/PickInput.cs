using System;

namespace Etupirka.Domain.External.Entities.Fsti
{
    public class PickInput
    {
        public string OrderType { get; set; }
        public string IssueType { get; set; }
        public string OrderNumber { get; set; }
        public int LineNumber { get; set; }
        public string ComponentLineType { get; set; }
        public string PointOfUseId { get; set; }
        public string OperationSequenceNumber { get; set; }
        public string ItemNumber { get; set; }
        public decimal RequiredQuantity { get; set; }

        public string QuantityType { get; set; }
    }
}
