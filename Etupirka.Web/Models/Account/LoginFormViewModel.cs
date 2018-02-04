using System.Collections.Generic;
using System.Web.Mvc;

namespace Etupirka.Web.Models.Account
{
    public class LoginFormViewModel
    {
        public SelectList TenancyNames { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsMultiTenancyEnabled { get; set; }
    }
}