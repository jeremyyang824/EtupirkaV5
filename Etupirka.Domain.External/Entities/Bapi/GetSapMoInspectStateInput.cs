using System;

namespace Etupirka.Domain.External.Entities.Bapi
{
    public class GetSapMoInspectStateInput
    {
        /// <summary>
        /// 生产任务号 最大长度12
        /// </summary>
        public string MOrderNumber { get; set; }

        /// <summary>
        /// 工序号 最大长度4
        /// </summary>
        public string OperationSeqnNumber { get; set; }
    }
}
