using System.Threading.Tasks;
using CoolShop.Admin.Services;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class LoginController : BaseController
    {
        private LoginService LoginService { get; }

        public LoginController(LoginService loginService)
        {
            LoginService = loginService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var redirect = GetParam<string>("redirect");
            if (string.IsNullOrWhiteSpace(redirect))
            {
                redirect = Url.ActionLink("Index", "Index");
            }

            ViewBag.redirect = redirect;
            return View();
        }

        public async Task<JsonResult> Do()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "login":
                {
                    var userName = GetParam<string>("userName").CheckRequired("请输入用户名");
                    var passWord = GetParam<string>("passWord").CheckRequired("请输入密码");
                    await LoginService.AdminLogin(userName, passWord);
                    break;
                }
                case "out":
                {
                    LoginService.AdminOut();
                    break;
                }
            }

            return Result();
        }
    }
}