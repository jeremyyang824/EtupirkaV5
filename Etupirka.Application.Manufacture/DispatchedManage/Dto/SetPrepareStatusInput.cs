using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    public class SetPrepareStatusInput
    {
        ///// <summary>
        ///// 流程类型
        ///// </summary>
        //public string JobType { get; set; }

        /// <summary>
        /// 机台任务ID 就是 派工单和工单的关系表主键
        /// </summary>
        public int PrepareInfoId { get; set; }
    }
}
