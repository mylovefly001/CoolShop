using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Admin.Entities;
using CoolShop.Admin.Wrappers;
using CoolShop.Core.Enum;
using CoolShop.Core.Library;
using CoolShop.Model;
using CoolShop.Repository;
using Newtonsoft.Json;
using Exception = CoolShop.Core.Extend.Exception;

namespace CoolShop.Admin.Services
{
    public class ManageService : BaseService
    {
        private SysMenuRepository SysMenuRepository { get; }

        public ManageService(SysMenuRepository sysMenuRepository)
        {
            SysMenuRepository = sysMenuRepository;
        }

        #region 系统菜单

        /// <summary>
        /// 获取系统菜单信息
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysMenuModel> GetSysMenuInfo(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await SysMenuRepository.GetInfo(func);
        }

        /// <summary>
        /// 获取系统菜单列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysMenuModel>> GetSysMenuList(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await SysMenuRepository.GetList(func);
        }

        /// <summary>
        /// 获取系统菜单树
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuTreeEntity>> GetSysMenuTree(Expression<Func<SysMenuModel, bool>> func = null)
        {
            var rsList = await SysMenuRepository.GetList(func);
            var dic = rsList.ToDictionary(t => t.Id, t => new MenuTreeEntity
            {
                Id = t.Id, Text = t.Name, Icon = t.Icon, Status = t.Status, Type = t.Type, Pid = t.Pid
            });
            foreach (var entity in dic.Values.Where(t => dic.ContainsKey(t.Pid)))
            {
                dic[entity.Pid].Child.Add(entity);
            }

            return dic.Values.Where(t => t.Pid == 0);
        }


        /// <summary>
        /// 添加一个系统菜单
        /// </summary>
        /// <param name="sysMenuWrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InsertSysMenu(SysMenuWrapper sysMenuWrapper)
        {
            var sysMenuModel = new SysMenuModel
            {
                Name = sysMenuWrapper.Name,
                Pid = sysMenuWrapper.Pid,
                Sort = sysMenuWrapper.Sort,
                Type = sysMenuWrapper.Type,
                Status = sysMenuWrapper.Status
            };
            if (sysMenuWrapper.Type == 1)
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Icon))
                {
                    throw new Exception("图标类名不得为空");
                }

                sysMenuModel.Icon = sysMenuWrapper.Icon;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Mctr))
                {
                    throw new Exception("controller名称不得为空");
                }

                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Mact))
                {
                    throw new Exception("action名称不得为空");
                }

                sysMenuModel.Mctr = sysMenuWrapper.Mctr;
                sysMenuModel.Mact = sysMenuWrapper.Mact;
                sysMenuModel.Mkey = sysMenuWrapper.Mctr.Md5(sysMenuWrapper.Mact);
            }

            if (await SysMenuRepository.Insert(sysMenuModel) <= 0)
            {
                throw new Exception("插入菜单信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="sysMenuWrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateSysMenu(SysMenuWrapper sysMenuWrapper)
        {
            var rsSysMenuInfo = await GetSysMenuInfo(t => t.Id == sysMenuWrapper.Id);
            if (rsSysMenuInfo == null)
            {
                throw new Exception("获取菜单信息失败", StatusCodeEnum.LogicErr);
            }

            rsSysMenuInfo.Name = sysMenuWrapper.Name;
            rsSysMenuInfo.Pid = sysMenuWrapper.Pid;
            rsSysMenuInfo.Sort = sysMenuWrapper.Sort;
            rsSysMenuInfo.Type = sysMenuWrapper.Type;
            rsSysMenuInfo.Status = sysMenuWrapper.Status;
            if (sysMenuWrapper.Type == 1)
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Icon))
                {
                    throw new Exception("图标类名不得为空");
                }

                rsSysMenuInfo.Mctr = string.Empty;
                rsSysMenuInfo.Mact = string.Empty;
                rsSysMenuInfo.Mkey = string.Empty;
                rsSysMenuInfo.Icon = sysMenuWrapper.Icon;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Mctr))
                {
                    throw new Exception("controller名称不得为空");
                }

                if (string.IsNullOrWhiteSpace(sysMenuWrapper.Mact))
                {
                    throw new Exception("action名称不得为空");
                }

                rsSysMenuInfo.Mctr = sysMenuWrapper.Mact;
                rsSysMenuInfo.Mact = sysMenuWrapper.Mact;
                rsSysMenuInfo.Mkey = sysMenuWrapper.Mctr.Md5(sysMenuWrapper.Mact);
                rsSysMenuInfo.Icon = string.Empty;
            }

            if (await SysMenuRepository.Update(rsSysMenuInfo) <= 0)
            {
                throw new Exception("更新菜单信息失败", StatusCodeEnum.LogicErr);
            }
        }
        
      /// <summary>
      /// 删除多个
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
        public async Task DeleteSysMenu(string ids)
        {
            var val = JsonConvert.DeserializeObject<List<int>>(ids);
            if (!val.Any())
            {
                throw new Exception("请选择需要删除的菜单ID");
            }

            if (await SysMenuRepository.Delete(val) <= 0)
            {
                throw new Exception("删除菜单信息失败", StatusCodeEnum.LogicErr);
            }
        }


        #endregion

        #region 管理组设置

        

        #endregion
    }
}