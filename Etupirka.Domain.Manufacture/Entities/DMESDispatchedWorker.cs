using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 定时任务 执行情况日志
    /// 拉取MES新下发的工票，更新本地齐备性流程状态
    /// </summary>
    public class DMESDispatchedWorker : CreationAuditedEntity
    {
        /// <summary>
        /// 执行来源（定时任务，手动）DMESDispatchedWorkerType
        /// </summary>
        public string WorkerType { get; set; }

        /// <summary>
        /// 同步开始时间
        /// </summary>
        public DateTime WorkerStartDate { get; set; }

        /// <summary>
        /// 同步完成时间
        /// </summary>
        public DateTime? WorkerFinishDate { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsWorkerSuccess { get; set; }

        /// <summary>
        /// 接口执行反馈消息
        /// </summary>
        public string WorkerResultMessage { get; set; }
    }
}
