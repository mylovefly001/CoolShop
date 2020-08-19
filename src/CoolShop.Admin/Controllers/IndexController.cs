using CoolShop.Admin.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class IndexController : BaseController
    {
        [Auth]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Auth]
        [HttpGet]
        public ActionResult Icon()
        {
            return View();
        }
    }
}