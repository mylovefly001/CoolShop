using System;
using System.Threading.Tasks;
using CoolShop.Admin.Attributes;
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
        [HttpGet]
        [Auth(Ctr = "Manage", Act = "MenuList")]
        public async Task<IActionResult> MenuList()
        {
            ViewBag.rsSysMenuList = await ManageService.GetSysMenuList(t => t.Pid == 0);
            return View();
        }

        /// <summary>
        /// 菜单具体操作
        /// </summary>
        /// <returns></returns>
        [Auth(Ctr = "Manage", Act = "MenuList")]
        public async Task<JsonResult> MenuDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var sysMenuWrapper = GetParam<SysMenuWrapper>(t => { t.Name.CheckRequired("请输入菜单名称"); });
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
                    return Result(rsSysMenuInfo);
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

        [HttpGet]
        [Auth(Ctr = "Manage", Act = "AdminGroupList")]
        public async Task<IActionResult> AdminGroupList()
        {
            ViewBag.rsSysMenuList = (await ManageService.GetSysMenuTree()).EncoderJson();
            return View();
        }

        [Auth(Ctr = "Manage", Act = "AdminGroupList")]
        public async Task<JsonResult> AdminGroupDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var sysAdminGroupWrapper = GetParam<SysAdminGroupWrapper>(t => { t.Name.CheckRequired("管理组名称不得为空"); });
                    await ManageService.InsertSysAdminGroup(sysAdminGroupWrapper);
                    break;
                }
                case "edit":
                {
                    var sysAdminGroupWrapper = GetParam<SysAdminGroupWrapper>(t =>
                    {
                        t.Id.CheckId("请输入管理组ID");
                        t.Name.CheckRequired("管理组名称不得为空");
                    });
                    await ManageService.UpdateSysAdminGroup(sysAdminGroupWrapper);
                    break;
                }
                case "del":
                {
                    var id = GetParam<int>("id").CheckId("请选择管理组ID");
                    await ManageService.DeleteSysAdminGroup(id);
                    break;
                }
                case "list":
                {
                    var rsSysAdminGroupList = await ManageService.GetSysAdminGroupList();
                    return Result(rsSysAdminGroupList);
                }
            }

            return Result();
        }

        #endregion

        #region 管理员设置

        [HttpGet]
        [Auth(Ctr = "Manage", Act = "AdminList")]
        public async Task<IActionResult> AdminList()
        {
            ViewBag.rsSysAdminGroupList = await ManageService.GetSysAdminGroupList();
            return View();
        }

        [Auth(Ctr = "Manage", Act = "AdminList")]
        public async Task<JsonResult> AdminDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var sysAdminWrapper = GetParam<SysAdminWrapper>(t =>
                    {
                        t.UserName.CheckAccount("管理员名称：4~16位小写英文字母与数字");
                        t.RealName.CheckRequired("请输入真实姓名");
                        t.PassWord.CheckPass("管理员密码：8~16位任意字符");
                    });
                    await ManageService.InsertSysAdmin(sysAdminWrapper);
                    break;
                }
                case "edit":
                {
                    var sysAdminWrapper = GetParam<SysAdminWrapper>(t =>
                    {
                        t.Id.CheckId("请输入管理员ID");
                        t.UserName.CheckAccount("管理员名称：4~16位小写英文字母与数字");
                        t.RealName.CheckRequired("请输入真实姓名");
                    });
                    await ManageService.UpdateSysAdmin(sysAdminWrapper);
                    break;
                }
                case "del":
                {
                    var id = GetParam<int>("id").CheckId("请输入管理员ID");
                    await ManageService.DeleteSysAdmin(id);
                    break;
                }
                case "info":
                {
                    var id = GetParam<int>("id").CheckId("请输入管理员ID");
                    var rsSysAdminInfo = await ManageService.GetSysAdminInfo(t => t.Id == id);
                    return Result(rsSysAdminInfo);
                }
                case "list":
                {
                    var rsSysAdminList = await ManageService.GetSysAdminList();
                    return Result(rsSysAdminList);
                }
            }

            return Result();
        }

        #endregion
    }
}