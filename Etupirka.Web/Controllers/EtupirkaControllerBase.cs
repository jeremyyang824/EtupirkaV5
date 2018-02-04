using System;
using System.IO;
using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Etupirka.Application.Portal.Dto;
using Etupirka.Domain.Portal;
using Microsoft.AspNet.Identity;

namespace Etupirka.Web.Controllers
{
    /// <summary>
    /// Mvc Controller基类
    /// </summary>
    public abstract class EtupirkaControllerBase : AbpController
    {
        protected EtupirkaControllerBase()
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
