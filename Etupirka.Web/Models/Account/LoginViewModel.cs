using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Runtime.Validation;

namespace Etupirka.Web.Models.Account
{
    public class LoginViewModel
    {
        public string TenancyName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}