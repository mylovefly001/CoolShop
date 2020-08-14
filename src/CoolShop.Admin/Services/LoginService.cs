using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolShop.Admin.Entities;
using CoolShop.Core.Enum;
using CoolShop.Core.Extend;
using CoolShop.Core.Library;
using CoolShop.Model;
using CoolShop.Repository;
using Microsoft.AspNetCore.Http;
using Exception = CoolShop.Core.Extend.Exception;

namespace CoolShop.Admin.Services
{
    public class LoginService : BaseService
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        private SysMenuRepository SysMenuRepository { get; }
        private SysAdminGroupRepository SysAdminGroupRepository { get; }
        private SysAdminRepository SysAdminRepository { get; }

        public LoginService(IHttpContextAccessor httpContextAccessor, SysMenuRepository sysMenuRepository, SysAdminGroupRepository sysAdminGroupRepository, SysAdminRepository sysAdminRepository)
        {
            HttpContextAccessor = httpContextAccessor;
            SysMenuRepository = sysMenuRepository;
            SysAdminGroupRepository = sysAdminGroupRepository;
            SysAdminRepository = sysAdminRepository;
        }

        private async Task<List<SysMenuModel>> GetMenuList(ICollection<int> ids)
        {
            var rsSysMenuList = await SysMenuRepository.GetList(t => ids.Contains(t.Id));
            var hasNew = false;
            foreach (var menuModel in rsSysMenuList.Where(t => t.Pid > 0 && !ids.Contains(t.Pid)))
            {
                ids.Add(menuModel.Pid);
                hasNew = true;
            }

            if (hasNew)
            {
                rsSysMenuList = await SysMenuRepository.GetList(t => ids.Contains(t.Id));
            }

            return rsSysMenuList;
        }

        private async Task<List<SysMenuModel>> GetMenuList(SysAdminGroupModel sysAdminGroupModel)
        {
            if (sysAdminGroupModel.IsRoot == 1)
            {
                return await SysMenuRepository.GetList(t => t.Status == 1);
            }

            var ids = sysAdminGroupModel.Detail.DecoderJson<List<int>>();
            return await GetMenuList(ids);
        }

        private async Task<(Dictionary<int, MenuDetailEntity>, List<string>)> GetMenuDetailsAndKeys(SysAdminGroupModel sysAdminGroupModel)
        {
            var menuKeys = new List<string>();
            var rsSysMenuList = await GetMenuList(sysAdminGroupModel);
            var menuDetails = rsSysMenuList.Where(t => t.Pid == 0).ToDictionary(t => t.Id, t => new MenuDetailEntity
            {
                MenuName = t.Name, MenuCtr = t.Mctr, MenuAct = t.Mact, MenuKey = t.Mkey, MenuType = t.Type, MenuPid = t.Pid, MenuIcon = t.Icon
            });
            foreach (var menuModel in rsSysMenuList)
            {
                if (menuDetails.ContainsKey(menuModel.Pid))
                {
                    menuDetails[menuModel.Pid].MenuDetail.Add(new MenuDetailEntity
                    {
                        MenuName = menuModel.Name, MenuCtr = menuModel.Mctr, MenuAct = menuModel.Mact, MenuKey = menuModel.Mkey,
                        MenuType = menuModel.Type, MenuPid = menuModel.Pid, MenuIcon = menuModel.Icon
                    });
                }

                if (menuModel.Type == 2)
                {
                    menuKeys.Add(menuModel.Mkey);
                }
            }

            return (menuDetails, menuKeys);
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AdminLogin(string userName, string passWord)
        {
            var rsSysAdminInfo = await SysAdminRepository.GetInfo(t => t.UserName == userName);
            if (rsSysAdminInfo == null)
            {
                throw new Exception("获取管理员信息失败", StatusCodeEnum.LogicErr);
            }

            var saltPass = passWord.Md5(rsSysAdminInfo.Salt);
            if (!string.Equals(saltPass, rsSysAdminInfo.PassWord, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("获取管理员信息失败", StatusCodeEnum.LogicErr);
            }

            if (rsSysAdminInfo.Status <= 0)
            {
                throw new Exception("该管理员已禁用，禁止登录", StatusCodeEnum.LogicErr);
            }

            if (rsSysAdminInfo.GroupId <= 0)
            {
                throw new Exception("该管理员未分配管理组，禁止登录", StatusCodeEnum.LogicErr);
            }

            var rsSysAdminGroupInfo = await SysAdminGroupRepository.GetInfo(t => t.Id == rsSysAdminInfo.GroupId);
            if (rsSysAdminGroupInfo == null)
            {
                throw new Exception("获取管理组信息失败", StatusCodeEnum.LogicErr);
            }

            if (rsSysAdminGroupInfo.Status <= 0)
            {
                throw new Exception("该管理组已禁用，禁止登录", StatusCodeEnum.LogicErr);
            }

            var (menuDetails, menuKeys) = await GetMenuDetailsAndKeys(rsSysAdminGroupInfo);
            HttpContextAccessor.HttpContext.Session.SetString(Session.AdminKey, new AdminSessionEntity
            {
                Id = rsSysAdminInfo.Id,
                IsRoot = rsSysAdminGroupInfo.IsRoot,
                UserName = rsSysAdminInfo.UserName,
                MenuKeys = menuKeys,
                MenuDetail = menuDetails.Select(t => t.Value).ToList(),
                LastLoginIp = HttpContextAccessor.HttpContext.Request.RealIp4(),
                LastLoginTime = DateTime.Now
            }.EncoderJson());
        }

        /// <summary>
        /// 管理员退出登录
        /// </summary>
        public void AdminOut()
        {
            HttpContextAccessor.HttpContext.Session.Remove(Session.AdminKey);
        }
    }
}