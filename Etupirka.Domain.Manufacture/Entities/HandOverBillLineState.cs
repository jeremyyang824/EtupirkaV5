using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单行状态
    /// </summary>
    public enum HandOverBillLineState
    {
        [Description("待处理")]
        Pending = 0,

        [Description("已接收")]
        Received = 1,

        [Description("已退回")]
        Rejected = 2,

        [Description("已转送")]
        Transfer = 3,
    }
}
