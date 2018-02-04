using System;
using System.ComponentModel;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接单转入类型
    /// </summary>
    public enum HandOverTargetType
    {
        [Description("部门交接")]
        Department = 0,

        [Description("供方交接")]
        Supplier = 1,
    }
}
