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
    /// 工作中心搜索条件
    /// </summary>
    public class FindWorkCentersInput : PagedAndFilteredInput, IShouldNormalize
    {
        /// <summary>
        /// 工作中心编码(可空)
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称(可空)
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 订单号(可空)
        /// </summary>
        public string OrderNumber { get; set; }

        public void Normalize()
        {
            this.WorkCenterCode = WorkCenterCode?.Trim();
            this.WorkCenterName = WorkCenterName?.Trim();
            this.OrderNumber = OrderNumber?.Trim();
        }
    }
}
