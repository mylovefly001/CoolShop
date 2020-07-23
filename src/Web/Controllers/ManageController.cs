using System;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Web.Services;
using CoolShop.Web.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Web.Controllers
{
    public class ManageController : BaseController
    {
        private ManageService ManageService { get; }

        public ManageController(ManageService manageService)
        {
            ManageService = manageService;
        }

        #region 菜单组

        public async Task<IActionResult> MenuList()
        {
            var rsSysMenuList = await ManageService.GetSysMenuList(t => t.Type == 1);
            ViewBag.rsSysMenuList = rsSysMenuList.EncoderJson();
            return View();
        }

        public async Task<JsonResult> MenuDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                    var sysMenuWrapper = GetParam<SysMenuWrapper>(t => { t.MenuName.CheckRequired("请输入菜单名称"); });
                    await ManageService.InsertSysMenu(sysMenuWrapper);
                    break;
                case "edit":
                    break;
                case "dels":
                    break;
                case "info":
                    var id = GetParam<int>("id").CheckId("请输入菜单ID");
                    var rsSysMenuInfo = await ManageService.GetSysMenuInfo(t => t.Id == id);
                    return Result(rsSysMenuInfo);
                case "list":
                    var rsSysMenuList = await ManageService.GetSysMenuTree();
                    return Result(rsSysMenuList);
            }

            return Result();
        }

        #endregion

        #region 超级管理员

        public IActionResult AdminGroupList()
        {
            return View();
        }

        public async Task<JsonResult> AdminGroupDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                    break;
                case "edit":
                    break;
                case "dels":
                    break;
                case "getlist":
                    var rsSysMenuList = await ManageService.GetSysMenuList();
                    return Result(rsSysMenuList);
            }

            return Result();
        }

        #endregion
    }
}