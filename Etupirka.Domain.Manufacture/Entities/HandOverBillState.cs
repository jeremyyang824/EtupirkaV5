using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单创建状态
    /// </summary>
    public enum HandOverBillState
    {
        [Description("草稿")]
        Draft = 0,

        [Description("已转出")]
        Published = 1,

        [Description("已完成")]
        Completed = 2,
    }
}
