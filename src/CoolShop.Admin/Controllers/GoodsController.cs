using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class GoodsController : BaseController
    {
        /// <summary>
        /// 模型管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ModelList()
        {
            return View();
        }
    }
}