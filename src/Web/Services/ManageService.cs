using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Entity;
using CoolShop.Core.Enum;
using CoolShop.Core.Library;
using CoolShop.Model;
using CoolShop.Repository;
using CoolShop.Web.Wrappers;
using Exception = CoolShop.Core.Extend.Exception;

namespace CoolShop.Web.Services
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
        /// 插入信息
        /// </summary>
        /// <param name="sysMenuWrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InsertSysMenu(SysMenuWrapper sysMenuWrapper)
        {
            var sysMenuModel = new SysMenuModel
            {
                Name = sysMenuWrapper.MenuName,
                Pid = sysMenuWrapper.MenuGroupId,
                Sort = sysMenuWrapper.MenuSort,
                Type = sysMenuWrapper.MenuType,
                Status = sysMenuWrapper.Status
            };
            if (sysMenuWrapper.MenuType == 1)
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.MenuIcon))
                {
                    throw new Exception("图标类名不得为空");
                }

                sysMenuModel.Icon = sysMenuWrapper.MenuIcon;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sysMenuWrapper.MenuCtr))
                {
                    throw new Exception("controller名称不得为空");
                }

                if (string.IsNullOrWhiteSpace(sysMenuWrapper.MenuAct))
                {
                    throw new Exception("action名称不得为空");
                }

                sysMenuModel.Mctr = sysMenuWrapper.MenuCtr;
                sysMenuModel.Mact = sysMenuWrapper.MenuAct;
                sysMenuModel.Mkey = sysMenuWrapper.MenuCtr.Md5(sysMenuWrapper.MenuAct);
            }

            if (await SysMenuRepository.Insert(sysMenuModel) <= 0)
            {
                throw new Exception("插入菜单信息失败", ErrorEnum.LogicErr);
            }
        }

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
                Id = t.Id, Name = t.Name, Icon = t.Icon, Mact = t.Mact, Mctr = t.Mctr, Mkey = t.Mkey,
                Pid = t.Pid, Sort = t.Sort, Status = t.Status, Type = t.Type
            });
            foreach (var model in dic.Values.Where(t => dic.ContainsKey(t.Pid)))
            {
                dic[model.Pid].Child.Add(new MenuTreeEntity
                {
                    
                });
            }

            return dic.Values.Where(t => t.Pid == 0);
        }

        #endregion
    }
}