using Etupirka.Domain.External.Entities.Wintool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Wintool
{
    public class WinToolArchiveResult
    {
        public bool IsSuccess { get; set; }

        public List<ArchiveBeanOutput> Files { get; set; }

        public string ArchiveType { get; set; }

        public WinToolArchiveResult()
        {
            this.IsSuccess = false;
            this.Files = new List<ArchiveBeanOutput>() { };
            this.ArchiveType = "";
        }

        public WinToolArchiveResult(bool isSuccess, List<ArchiveBeanOutput> files, string archiveType)
        {
            this.IsSuccess = isSuccess;
            this.Files = files;
            this.ArchiveType = archiveType;
        }
    }
}
