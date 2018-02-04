using Abp.Application.Services.Dto;

namespace Etupirka.Application.Portal.Dto
{
    /// <summary>
    /// 分页并排序方法输入参数
    /// </summary>
    public class PagedAndSortedInput : PagedInput, ISortedResultRequest
    {
        /// <summary>
        /// 排序信息：排序字段 ASC/DESC （多个字段以逗号','分隔）
        /// </summary>
        public string Sorting { get; set; }
    }
}
