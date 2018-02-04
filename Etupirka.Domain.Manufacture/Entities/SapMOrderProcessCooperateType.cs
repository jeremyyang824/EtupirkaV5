using System;
using System.ComponentModel;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// SAP工艺外协类型
    /// </summary>
    public enum SapMOrderProcessCooperateType
    {
        [Description("东厂外协")]
        ToForthShift = 0,

        [Description("供方外协")]
        ToOutsideSupplier = 1,
    }
}
