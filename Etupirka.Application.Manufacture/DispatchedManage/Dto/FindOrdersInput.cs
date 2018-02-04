using Abp.Runtime.Validation;
using Etupirka.Application.Portal.Dto;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    public class FindOrdersInput : PagedAndFilteredInput
    {
        /// <summary>
        /// 工作中心 主键
        /// </summary>
        [Required]
        public int WorkCenterID { get; set; }

    }
}
