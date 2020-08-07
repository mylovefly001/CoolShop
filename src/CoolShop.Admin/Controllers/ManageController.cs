using System;
using System.Threading.Tasks;
using CoolShop.Admin.Services;
using CoolShop.Admin.Wrappers;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class ManageController : BaseController
    {
        private ManageService ManageService { get; }

        public ManageController(ManageService manageService)
        {
            ManageService = manageService;
        }

        #region 菜单部分

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MenuList()
        {
            ViewBag.rsSysMenuList = await ManageService.GetSysMenuList(t => t.Pid == 0);
            return View();
        }

        /// <summary>
        /// 菜单具体操作
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> MenuDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var sysMenuWrapper = GetParam<SysMenuWrapper>(t =>
                    {
                        t.Name.CheckRequired("请输入菜单名称");
                    });
                    await ManageService.InsertSysMenu(sysMenuWrapper);
                    break;
                }
                case "edit":
                {
                    var sysMenuWrapper = GetParam<SysMenuWrapper>(t =>
                    {
                        t.Id.CheckId("菜单ID不得为空");
                        t.Name.CheckRequired("请输入菜单名称");
                    });
                    await ManageService.UpdateSysMenu(sysMenuWrapper);
                    break;
                }
                case "dels":
                    var ids = GetParam<string>("ids");
                    await ManageService.DeleteSysMenu(ids);
                    break;
                case "info":
                {
                    var id = GetParam<int>("id").CheckId("菜单ID不得为空");
                    var rsSysMenuInfo = await ManageService.GetSysMenuInfo(t => t.Id == id);
                    return rsSysMenuInfo == null ? Result("获取菜单信息失败") : Result(rsSysMenuInfo);
                }
                case "tree":
                {
                    var rsSysMenuTree = await ManageService.GetSysMenuTree();
                    return Result(rsSysMenuTree);
                }
            }

            return Result();
        }

        #endregion

        #region 管理组设置

        public IActionResult AdminGroupList()
        {
            return View();
        }

        #endregion
    }
}