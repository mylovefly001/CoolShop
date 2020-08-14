using CoolShop.Admin.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class IndexController : BaseController
    {
        [Auth]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Auth]
        [HttpGet]
        public IActionResult Icon()
        {
            return View();
        }
    }
}