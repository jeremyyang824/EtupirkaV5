using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Winchill;
using Etupirka.Domain.External.Entities.Wintool;
using Etupirka.Domain.External.Wintool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Repositories
{
    public interface IWintoolApiRepository : IRepository
    {
        Task<WinToolResult> WintoolCreateToolingJob(CreateJobInput input);
        Task<WinToolResult> WintoolCreateNCJob(CreateJobInput input);

        Task<WinToolArchiveResult> WintoolGetArchive(GetArchiveInput input);

    }
}
