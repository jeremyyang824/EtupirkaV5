using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Etupirka.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : EtupirkaControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}