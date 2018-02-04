using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单行质检情况
    /// </summary>
    public enum HandOverBillLineInspectState
    {
        [Description("")]
        Pending = 0,

        [Description("已检验")]
        Inspected = 1,

        [Description("未检验")]
        UnInspected = 2,

        [Description("异常错误")]
        Error = 3,
    }
}
