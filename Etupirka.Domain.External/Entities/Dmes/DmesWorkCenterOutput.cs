using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Entities.Dmes
{
    public class DmesWorkCenterOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }
    }
}
