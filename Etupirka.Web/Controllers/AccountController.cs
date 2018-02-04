using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Web.Models;
using Etupirka.Application.Portal.MultiTenancy;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.Authorization;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;
using Etupirka.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Etupirka.Web.Controllers
{
    public class AccountController : EtupirkaControllerBase
    {
        private readonly SysTenantManager _tenantManager;
        private readonly SysUserManager _userManager;
        private readonly SysRoleManager _roleManager;
        private readonly LogInManager _logInManager;
        private readonly ITenantAppService _tenantAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController(
            SysTenantManager tenantManager,
            SysUserManager userManager,
            SysRoleManager roleManager,
            LogInManager logInManager,
            ITenantAppService tenantAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logInManager = logInManager;
            _tenantAppService = tenantAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;
        }

        #region Login / Logout

        public async Task<ActionResult> LinkLogin(string username, string returnUrl = "", string returnUrlHash = "")
        {
            var loginResult = await _logInManager.LoginAsync(username, null);
            if (loginResult.Result != AbpLoginResultType.Success)
                throw CreateExceptionForFailedLoginAttempt(loginResult.Result, username, null);

            await SignInAsync(loginResult.User, loginResult.Identity, false);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }
            return Redirect(returnUrl);
        }

        public async Task<ActionResult> Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return View(
                new LoginFormViewModel
                {
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                    TenancyNames = _multiTenancyConfig.IsEnabled ? (await this.GetAllTenancyNames()) : null,
                    ReturnUrl = returnUrl
                });
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();

            var loginResult = await GetLoginResultAsync(
                loginModel.UserName,
                loginModel.Password,
                loginModel.TenancyName);


            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }
            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        private async Task<SelectList> GetAllTenancyNames()
        {
            List<NameValueDto> tenancyNames = await _tenantAppService.GetActiveTenancyNames();
            tenancyNames.Add(new NameValueDto { Name = EtupirkaPortalConsts.HostFullName, Value = (string)null });   //Add Host
            return new SelectList(tenancyNames, "Value", "Name");
        }

        private async Task<AbpLoginResult<SysTenant, SysUser>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SignInAsync(SysUser user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        #endregion

    }
}