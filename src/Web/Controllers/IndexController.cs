using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Web.Controllers
{
    public class IndexController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Icon()
        {
            return View();
        }
    }
}