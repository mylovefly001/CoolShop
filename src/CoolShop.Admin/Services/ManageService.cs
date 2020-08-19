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

        private SysAdminGroupRepository SysAdminGroupRepository { get; }

        private SysAdminRepository SysAdminRepository { get; }

        public ManageService(SysMenuRepository sysMenuRepository, SysAdminGroupRepository sysAdminGroupRepository, SysAdminRepository sysAdminRepository)
        {
            SysMenuRepository = sysMenuRepository;
            SysAdminGroupRepository = sysAdminGroupRepository;
            SysAdminRepository = sysAdminRepository;
        }

        #region 系统菜单

        /// <summary>
        /// 获取系统菜单信息
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysMenuModel> GetSysMenuInfo(Expression<Func<SysMenuModel, bool>> func = null)
        {
            var rs = await SysMenuRepository.GetInfo(func);
            if (rs == null)
            {
                throw new Exception("获取菜单信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
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

                rsSysMenuInfo.Mctr = sysMenuWrapper.Mctr;
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

        /// <summary>
        /// 获取管理组列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysAdminGroupModel>> GetSysAdminGroupList(Expression<Func<SysAdminGroupModel, bool>> func = null)
        {
            return await SysAdminGroupRepository.GetList(func);
        }

        public async Task<SysAdminGroupModel> GetSysAdminGroupInfo(Expression<Func<SysAdminGroupModel, bool>> func = null)
        {
            var rs = await SysAdminGroupRepository.GetInfo(func);
            if (rs == null)
            {
                throw new Exception("获取管理组信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
        }

        /// <summary>
        /// 添加管理组
        /// </summary>
        /// <param name="sysAdminGroupWrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InsertSysAdminGroup(SysAdminGroupWrapper sysAdminGroupWrapper)
        {
            var tmpSysAdminGroupInfo = await SysAdminGroupRepository.GetInfo(t => t.Name == sysAdminGroupWrapper.Name);
            if (tmpSysAdminGroupInfo != null)
            {
                throw new Exception("该管理组名称已存在", StatusCodeEnum.LogicErr);
            }

            var sysAdminGroupModel = new SysAdminGroupModel
            {
                Name = sysAdminGroupWrapper.Name,
                Detail = sysAdminGroupWrapper.Detail,
                Status = sysAdminGroupWrapper.Status
            };
            if (await SysAdminGroupRepository.Insert(sysAdminGroupModel) <= 0)
            {
                throw new Exception("插入管理组信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 更新一条信息
        /// </summary>
        /// <param name="sysAdminGroupWrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateSysAdminGroup(SysAdminGroupWrapper sysAdminGroupWrapper)
        {
            var tmpSysAdminGroupInfo = await SysAdminGroupRepository.GetInfo(t => t.Name == sysAdminGroupWrapper.Name && t.Id != sysAdminGroupWrapper.Id);
            if (tmpSysAdminGroupInfo != null)
            {
                throw new Exception("该管理组名称已存在", StatusCodeEnum.LogicErr);
            }

            var rsSysAdminGroupInfo = await GetSysAdminGroupInfo(t => t.Id == sysAdminGroupWrapper.Id);
            if (rsSysAdminGroupInfo.IsRoot == 1)
            {
                throw new Exception("该管理组是超级管理组，禁止操作", StatusCodeEnum.LogicErr);
            }

            rsSysAdminGroupInfo.Name = sysAdminGroupWrapper.Name;
            rsSysAdminGroupInfo.Detail = sysAdminGroupWrapper.Detail;
            rsSysAdminGroupInfo.Status = sysAdminGroupWrapper.Status;
            if (await SysAdminGroupRepository.Update(rsSysAdminGroupInfo) <= 0)
            {
                throw new Exception("更新管理组信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteSysAdminGroup(int id)
        {
            if (await SysAdminGroupRepository.Delete(id) <= 0)
            {
                throw new Exception("删除管理组信息失败", StatusCodeEnum.LogicErr);
            }
        }

        #endregion

        #region 管理员设置

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysAdminModel>> GetSysAdminList(Expression<Func<SysAdminModel, bool>> func = null)
        {
            return await SysAdminRepository.GetList(func);
        }

        /// <summary>
        /// 获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysAdminModel> GetSysAdminInfo(int id)
        {
            var rs = await SysAdminRepository.GetInfo(id);
            if (rs == null)
            {
                throw new Exception("获取管理员信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
        }

        /// <summary>
        /// 获取一条信息
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysAdminModel> GetSysAdminInfo(Expression<Func<SysAdminModel, bool>> func = null)
        {
            var rs = await SysAdminRepository.GetInfo(func);
            if (rs == null)
            {
                throw new Exception("获取管理员信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="sysAdminWrapper"></param>
        /// <returns></returns>
        public async Task InsertSysAdmin(SysAdminWrapper sysAdminWrapper)
        {
            if (sysAdminWrapper.PassWord != sysAdminWrapper.SurePass)
            {
                throw new Exception("两次输入的密码不一致", StatusCodeEnum.LogicErr);
            }

            var tmpSysAdminInfo = await SysAdminRepository.GetInfo(t => t.UserName == sysAdminWrapper.UserName);
            if (tmpSysAdminInfo != null)
            {
                throw new Exception("该管理员名称已存在", StatusCodeEnum.LogicErr);
            }

            var salt = Common.CreateRandom(8);
            var sysAdminModel = new SysAdminModel
            {
                UserName = sysAdminWrapper.UserName,
                GroupId = sysAdminWrapper.GroupId,
                PassWord = sysAdminWrapper.PassWord.Md5(salt),
                RealName = sysAdminWrapper.RealName,
                Salt = salt,
                Status = sysAdminWrapper.Status
            };
            if (await SysAdminRepository.Insert(sysAdminModel) <= 0)
            {
                throw new Exception("插入管理员信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="oldPass"></param>
        /// <param name="sysAdminWrapper"></param>
        /// <returns></returns>
        public async Task SetSysAdmin(string oldPass, SysAdminWrapper sysAdminWrapper)
        {
            var rsSysAdminInfo = await GetSysAdminInfo(sysAdminWrapper.Id);
            if (!string.IsNullOrWhiteSpace(sysAdminWrapper.PassWord))
            {
                sysAdminWrapper.PassWord.CheckPass("管理员密码格式错误：8~16位任意字符");
                if (!sysAdminWrapper.PassWord.Equals(sysAdminWrapper.SurePass))
                {
                    throw new Exception("两次输入的密码不一致", StatusCodeEnum.LogicErr);
                }

                if (!rsSysAdminInfo.PassWord.Equals(oldPass.Md5(rsSysAdminInfo.Salt)))
                {
                    throw new Exception("旧密码不正确", StatusCodeEnum.LogicErr);
                }

                rsSysAdminInfo.PassWord = sysAdminWrapper.PassWord.Md5(rsSysAdminInfo.Salt);
            }

            rsSysAdminInfo.RealName = sysAdminWrapper.RealName;
            if (await SysAdminRepository.Update(rsSysAdminInfo) <= 0)
            {
                throw new Exception("更新管理员信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 更新一条信息
        /// </summary>
        /// <param name="sysAdminWrapper"></param>
        /// <returns></returns>
        public async Task UpdateSysAdmin(SysAdminWrapper sysAdminWrapper)
        {
            var tmpSysAdminInfo = await SysAdminRepository.GetInfo(t => t.UserName == sysAdminWrapper.UserName && t.Id != sysAdminWrapper.Id);
            if (tmpSysAdminInfo != null)
            {
                throw new Exception("该管理员名称已存在", StatusCodeEnum.LogicErr);
            }

            var rsSysAdminInfo = await GetSysAdminInfo(sysAdminWrapper.Id);
            if (!string.IsNullOrWhiteSpace(sysAdminWrapper.PassWord))
            {
                if (!sysAdminWrapper.PassWord.Equals(sysAdminWrapper.SurePass))
                {
                    throw new Exception("两次输入的密码不一致", StatusCodeEnum.LogicErr);
                }

                sysAdminWrapper.PassWord.CheckPass("管理员密码格式错误：8~16位任意字符");
                sysAdminWrapper.PassWord = sysAdminWrapper.PassWord.Md5(rsSysAdminInfo.Salt);
            }
            else
            {
                sysAdminWrapper.PassWord = rsSysAdminInfo.PassWord;
            }

            rsSysAdminInfo.GroupId = sysAdminWrapper.GroupId;
            rsSysAdminInfo.UserName = sysAdminWrapper.UserName;
            rsSysAdminInfo.RealName = sysAdminWrapper.RealName;
            rsSysAdminInfo.PassWord = sysAdminWrapper.PassWord;
            rsSysAdminInfo.Status = sysAdminWrapper.Status;
            if (await SysAdminRepository.Update(rsSysAdminInfo) <= 0)
            {
                throw new Exception("更新管理员信息失败", StatusCodeEnum.LogicErr);
            }
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteSysAdmin(int id)
        {
            if (await SysAdminRepository.Delete(id) <= 0)
            {
                throw new Exception("删除管理员信息失败", StatusCodeEnum.LogicErr);
            }
        }

        #endregion
    }
}