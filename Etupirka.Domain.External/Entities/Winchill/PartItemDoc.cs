using System;
using System.Collections.Generic;
using System.Linq;

namespace Etupirka.Domain.External.Entities.Winchill
{
    /// <summary>
    /// 零件图纸信息
    /// </summary>
    public class PartItemDoc
    {
        public string PartNumber { get; set; }

        public string PartVersion { get; set; }

        public string PartName { get; set; }

        public string DocNumber { get; set; }

        public string DocVersion { get; set; }

        public string DocName { get; set; }

        public string DownloadUrl3D { get; set; }

        public string DownloadUrl2D { get; set; }

        public DateTime PublishTime { get; set; }

        public string DocModifier { get; set; }

        public string Flag { get; set; }
    }
}
