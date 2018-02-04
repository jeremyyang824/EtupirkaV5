using Abp.IdentityFramework;
using Abp.UI;
using Abp.WebApi.Controllers;
using Etupirka.Domain.Portal;
using Microsoft.AspNet.Identity;

namespace Etupirka.WebApi.Api.Controllers
{
    /// <summary>
    /// WebApi Controller基类
    /// </summary>
    public class EtupirkaApiController : AbpApiController
    {
        protected EtupirkaApiController()
        {
            LocalizationSourceName = EtupirkaPortalConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
