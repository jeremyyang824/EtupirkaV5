using Abp.Runtime.Validation;
using Etupirka.Application.Portal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    /// <summary>
    /// 获取齐备性信息
    /// </summary>
    public class FindPrepareInfosInput : PagedAndFilteredInput, IShouldNormalize
    {
        /// <summary>
        /// 齐备性流程类型 all tl nc
        /// </summary>
        public string PrepareType { get; set; }

        /// <summary>
        /// 齐备性流程状态 1 准备中 2 已完成
        /// </summary>
        public string PrepareStatus { get; set; }

        public void Normalize()
        {
            this.PrepareType = PrepareType?.Trim();
            this.PrepareStatus = PrepareStatus?.Trim();
        }
    }
}
