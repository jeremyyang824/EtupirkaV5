using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Etupirka.Application.Portal.Dto
{
    /// <summary>
    /// 分页方法输入参数
    /// </summary>
    public class PagedInput : IPagedResultRequest
    {
        /// <summary>
        /// 最大返回记录数
        /// </summary>
        [Range(1, EtupirkaAppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        /// <summary>
        /// 跳过记录数
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public PagedInput()
        {
            MaxResultCount = EtupirkaAppConsts.DefaultPageSize;
        }
    }
}
