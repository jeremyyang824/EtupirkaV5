namespace Etupirka.Application.Portal.Dto
{
    /// <summary>
    /// 分页并过滤方法输入参数
    /// </summary>
    public class PagedAndFilteredInput : PagedInput
    {
        /// <summary>
        /// 过滤内容
        /// </summary>
        public string Filter { get; set; }
    }
}
