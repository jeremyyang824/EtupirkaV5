using System;

namespace Etupirka.Domain.External.Entities.Bapi
{
    public class GetSapMoInspectStateOutput
    {
        /// <summary>
        /// 1    质检控制码为ZQ01或ZQ02，且有质检记录。
        /// 2    质检控制码为ZQ01或ZQ02，无质检记录。
        /// 4    找不到生产任务对应的工序号。
        /// 0    质检控制码非ZQ01或ZQ02。
        /// </summary>
        public int InspectState { get; set; }

        /// <summary>
        /// 是否正常质检
        /// </summary>
        public bool IsInspected()
        {
            return this.InspectState == 1 || this.InspectState == 0;
        }
    }
}
