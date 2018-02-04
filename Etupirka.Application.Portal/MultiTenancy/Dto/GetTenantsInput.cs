using Abp.Runtime.Validation;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Application.Portal.MultiTenancy.Dto
{
    /// <summary>
    /// 获取租户输入参数
    /// </summary>
    public class GetTenantsInput : PagedAndSortedAndFilteredInput, IShouldNormalize
    {
        public bool? IsActive { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "TenancyName";    //默认按租户名排序
            }
        }
    }
}
