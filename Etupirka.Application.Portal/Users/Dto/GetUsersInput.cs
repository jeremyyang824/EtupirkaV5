using Abp.Runtime.Validation;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Application.Portal.Users.Dto
{
    /// <summary>
    /// 获取用户输入参数
    /// </summary>
    public class GetUsersInput : PagedAndSortedAndFilteredInput, IShouldNormalize
    {
        public bool? IsActive { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "UserName"; 
            }
        }
    }
}
