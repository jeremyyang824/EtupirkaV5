using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Entities.Dmes
{
    /// <summary>
    /// 拉取MES新下发的工票
    /// </summary>
    public class DmesGetDispatchedIdsInput
    {
        /// <summary>
        /// 最后执行时间
        /// </summary>
        public DateTime lastWorkerDate { get; set; }
    }
}
