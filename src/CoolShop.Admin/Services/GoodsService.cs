using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Admin.Wrappers;
using CoolShop.Core.Enum;
using CoolShop.Model;
using CoolShop.Repository;
using Exception = CoolShop.Core.Extend.Exception;

namespace CoolShop.Admin.Services
{
    public class GoodsService : BaseService
    {
        private GoodsTemplateRepository GoodsTemplateRepository { get; }
        private GoodsAttributeRepository GoodsAttributeRepository { get; }

        public GoodsService(GoodsTemplateRepository goodsTemplateRepository, GoodsAttributeRepository goodsAttributeRepository)
        {
            GoodsTemplateRepository = goodsTemplateRepository;
            GoodsAttributeRepository = goodsAttributeRepository;
        }


        #region 商品模板

        public async Task<GoodsTemplateModel> GetGoodsTemplateInfo(Expression<Func<GoodsTemplateModel, bool>> func = null)
        {
            var rs = await GoodsTemplateRepository.GetInfo(func);
            if (rs == null)
            {
                throw new Exception("获取商品模板信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
        }

        public async Task<List<GoodsTemplateModel>> GetGoodsTemplateList(Expression<Func<GoodsTemplateModel, bool>> func = null)
        {
            return await GoodsTemplateRepository.GetList(func);
        }

        public async Task InsertGoodsTemplate(string name, int sort, int status)
        {
            var tmpGoodsTemplateInfo = await GoodsTemplateRepository.GetInfo(t => t.Name == name);
            if (tmpGoodsTemplateInfo != null)
            {
                throw new Exception("该商品模板已存在", StatusCodeEnum.LogicErr);
            }

            var goodsTemplateModel = new GoodsTemplateModel
            {
                Name = name, Sort = sort, Status = status
            };
            if (await GoodsTemplateRepository.Insert(goodsTemplateModel) <= 0)
            {
                throw new Exception("添加商品模板信息失败", StatusCodeEnum.LogicErr);
            }
        }

        public async Task UpdateGoodsTemplate(int id, string name, int sort, int status)
        {
            var tmpGoodsTemplateInfo = await GoodsTemplateRepository.GetInfo(t => t.Id != id && t.Name == name);
            if (tmpGoodsTemplateInfo != null)
            {
                throw new Exception("该名称模板已存在", StatusCodeEnum.LogicErr);
            }

            var rsGoodsTemplateInfo = await GetGoodsTemplateInfo(t => t.Id == id);
            rsGoodsTemplateInfo.Name = name;
            rsGoodsTemplateInfo.Sort = sort;
            rsGoodsTemplateInfo.Status = status;
            if (await GoodsTemplateRepository.Update(rsGoodsTemplateInfo) <= 0)
            {
                throw new Exception("更新商品模板信息失败", StatusCodeEnum.LogicErr);
            }
        }

        public async Task DeleteGoodsTemplate(int id)
        {
            if (await GoodsTemplateRepository.Delete(id) <= 0)
            {
                throw new Exception("删除商品模板信息失败", StatusCodeEnum.LogicErr);
            }
        }

        #endregion

        #region 商品属性

        public async Task<GoodsAttributeModel> GetGoodsAttributeInfo(Expression<Func<GoodsAttributeModel, bool>> func = null)
        {
            var rs = await GoodsAttributeRepository.GetInfo(func);
            if (rs == null)
            {
                throw new Exception("获取商品属性信息失败", StatusCodeEnum.LogicErr);
            }

            return rs;
        }

        public async Task<List<GoodsAttributeModel>> GetGoodsAttributeList(Expression<Func<GoodsAttributeModel, bool>> func = null)
        {
            return await GoodsAttributeRepository.GetList(func);
        }

        public async Task InsertGoodsAttribute(GoodsAttributeWrapper goodsAttributeWrapper)
        {
            var tmpGoodsAttributeInfo = await GoodsAttributeRepository.GetInfo(t => t.Name == goodsAttributeWrapper.Name);
            if (tmpGoodsAttributeInfo != null)
            {
                throw new Exception("该商品属性已存在", StatusCodeEnum.LogicErr);
            }

            var goodsAttributeModel = new GoodsAttributeModel
            {
                Name = goodsAttributeWrapper.Name,
                Description = goodsAttributeWrapper.Description,
                Tid = goodsAttributeWrapper.Tid,
                Pid = goodsAttributeWrapper.Pid,
                Icon = goodsAttributeWrapper.Icon,
                InputMode = goodsAttributeWrapper.InputMode,
                InputVal = goodsAttributeWrapper.InputVal,
                Sort = goodsAttributeWrapper.Sort,
                Status = goodsAttributeWrapper.Status
            };
            if (await GoodsAttributeRepository.Insert(goodsAttributeModel) <= 0)
            {
                throw new Exception("添加商品属性信息失败", StatusCodeEnum.LogicErr);
            }
        }

        public async Task UpdateGoodsAttribute(GoodsAttributeWrapper goodsAttributeWrapper)
        {
            var tmpGoodsAttributeInfo = await GoodsAttributeRepository.GetInfo(
                t => t.Id != goodsAttributeWrapper.Id && t.Name == goodsAttributeWrapper.Name
            );
            if (tmpGoodsAttributeInfo != null)
            {
                throw new Exception("该商品属性已存在", StatusCodeEnum.LogicErr);
            }

            var rsGoodsAttributeInfo = await GetGoodsAttributeInfo(t => t.Id == goodsAttributeWrapper.Id);
            rsGoodsAttributeInfo.Name = goodsAttributeWrapper.Name;
            rsGoodsAttributeInfo.Description = goodsAttributeWrapper.Description;
            rsGoodsAttributeInfo.Tid = goodsAttributeWrapper.Tid;
            rsGoodsAttributeInfo.Pid = goodsAttributeWrapper.Pid;
            rsGoodsAttributeInfo.Icon = goodsAttributeWrapper.Icon;
            rsGoodsAttributeInfo.InputMode = goodsAttributeWrapper.InputMode;
            rsGoodsAttributeInfo.InputVal = goodsAttributeWrapper.InputVal;
            rsGoodsAttributeInfo.Sort = goodsAttributeWrapper.Sort;
            rsGoodsAttributeInfo.Status = goodsAttributeWrapper.Status;
            if (await GoodsAttributeRepository.Update(rsGoodsAttributeInfo) <= 0)
            {
                throw new Exception("更新商品属性信息失败", StatusCodeEnum.LogicErr);
            }
        }

        public async Task DeleteGoodsAttribute(int id)
        {
            if (await GoodsAttributeRepository.Delete(id) <= 0)
            {
                throw new Exception("删除商品属性信息失败", StatusCodeEnum.LogicErr);
            }
        }

        #endregion
    }
}