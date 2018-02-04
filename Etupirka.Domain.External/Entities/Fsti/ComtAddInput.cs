using System;
using System.Collections.Generic;

namespace Etupirka.Domain.External.Entities.Fsti
{
    public class ComtAddInput
    {
        public string CoNumber { get; set; }
        public string CustomerId { get; set; }
        public List<LineItem> ComtAddLines { get; set; }

        public class LineItem
        {
            public int CoLineNumber { get; set; }
            public string ItemNumber { get; set; }
            public decimal ItemOrderedQuantity { get; set; }
            public DateTime PromisedShipDate { get; set; }
            public int CoLineStatus { get; set; }
            public decimal ItemControllingNetUnitPrice { get; set; }

            public string TextLine1 { get; set; }
            public string TextLine2 { get; set; }
            public string TextLine3 { get; set; }
            public string TextLine4 { get; set; }

            public bool IsNeedAddLineText()
            {
                if (this.TextLine1 != null
                    || this.TextLine2 != null
                    || this.TextLine3 != null
                    || this.TextLine4 != null)
                    return true;
                return false;
            }
        }
    }
}
