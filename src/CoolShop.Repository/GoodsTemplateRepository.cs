using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class GoodsTemplateRepository : BaseRepository
    {
        public GoodsTemplateRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据ID删除一批数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> Delete(List<int> ids)
        {
            return await DbContext.Delete<GoodsTemplateModel>(ids).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<GoodsTemplateModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(GoodsTemplateModel model)
        {
            return await DbContext.Insert<GoodsTemplateModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(GoodsTemplateModel model)
        {
            return await DbContext.Update<GoodsTemplateModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<GoodsTemplateModel> GetInfo(Expression<Func<GoodsTemplateModel, bool>> func = null)
        {
            return await DbContext.Select<GoodsTemplateModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoodsTemplateModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<GoodsTemplateModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<GoodsTemplateModel>> GetList(Expression<Func<GoodsTemplateModel, bool>> func = null)
        {
            return await DbContext.Select<GoodsTemplateModel>().WhereIf(func != null, func)
                .OrderByDescending(t => t.Sort)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}