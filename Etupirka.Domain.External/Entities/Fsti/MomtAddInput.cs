using System;
using System.Collections.Generic;

namespace Etupirka.Domain.External.Entities.Fsti
{
    public class MomtAddInput
    {
        public string MoNumber { get; set; }
        public string Planner { get; set; }
        public string WorkCenter { get; set; }
        public string DeliverTo { get; set; }
        public List<LineItem> MomtAddLines { get; set; }

        public class LineItem
        {
            public string ItemNumber { get; set; }
            public string MoLineType { get; set; }
            public decimal ItemOrderedQuantity { get; set; }
            public int MoLineStatus { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime ScheduledDate { get; set; }

            public int MoLineNumber { get; set; }
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
