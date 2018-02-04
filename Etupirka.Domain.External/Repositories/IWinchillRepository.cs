using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Winchill;

namespace Etupirka.Domain.External.Repositories
{
    /// <summary>
    /// Whill图纸获取
    /// </summary>
    public interface IWinchillRepository : IRepository
    {
        /// <summary>
        /// 取得零部件记录
        /// </summary>
        Task<IEnumerable<PartItemDoc>> GetByPartItem(GetByPartItemInput input);

        /// <summary>
        /// 取得所有零件版本
        /// </summary>
        Task<IEnumerable<PartVersionOutput>> GetAllPartVersions();
    }
}
