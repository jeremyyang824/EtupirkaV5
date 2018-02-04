using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Dmes;

namespace Etupirka.Domain.External.Repositories
{
    /// <summary>
    /// 电气MES 外部数据库操作
    /// </summary>
    public interface IDMESWorkCenterRepository : IRepository
    {
        Task<DmesWorkCenterOutput> GetWorkCenter(int id);

        Task<IPagedResult<DmesWorkCenterOutput>> GetWorkCenters(DmesGetWorkCenterInput input);

        Task<IList<DmesWorkcenterMapOutput>> GetWorkCenterWinToolMaps();
    }
}
