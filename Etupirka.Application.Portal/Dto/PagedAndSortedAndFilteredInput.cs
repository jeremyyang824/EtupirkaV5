namespace Etupirka.Application.Portal.Dto
{
    /// <summary>
    /// 分页、排序、过滤方法输入参数
    /// </summary>
    public class PagedAndSortedAndFilteredInput : PagedAndSortedInput
    {
        /// <summary>
        /// 过滤内容
        /// </summary>
        public string Filter { get; set; }
    }
}
