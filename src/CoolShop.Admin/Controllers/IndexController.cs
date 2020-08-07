using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class IndexController : BaseController
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}