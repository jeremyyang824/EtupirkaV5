using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Entities.Wintool
{
    public class GetArchiveInput
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemNumber { get; set; }

        /// <summary>
        /// 类型 CNCArchive/GetToolPresetPdf，GetDrawings，GetCAPPs
        /// </summary>
        public int ArchiveType { get; set; }

        public string buildArchiceFunc
        {
            get
            {
                return Enum.GetName(typeof(ArchiveTypeEnum), ArchiveType).ToString();
            }
        }
    }
    
    public enum ArchiveTypeEnum : int
    {
        GetToolPresetPdf = 1,
        GetDrawings = 2,
        GetCAPPs = 3,
    }
}
