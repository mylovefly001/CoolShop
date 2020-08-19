using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class GoodsAttributeRepository : BaseRepository
    {
        public GoodsAttributeRepository(DbContext dbContext) : base(dbContext)
        {
        }
        
         /// <summary>
        /// 根据ID删除一批数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> Delete(List<int> ids)
        {
            return await DbContext.Delete<GoodsAttributeModel>(ids).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<GoodsAttributeModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(GoodsAttributeModel model)
        {
            return await DbContext.Insert<GoodsAttributeModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(GoodsAttributeModel model)
        {
            return await DbContext.Update<GoodsAttributeModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<GoodsAttributeModel> GetInfo(Expression<Func<GoodsAttributeModel, bool>> func = null)
        {
            return await DbContext.Select<GoodsAttributeModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoodsAttributeModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<GoodsAttributeModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<GoodsAttributeModel>> GetList(Expression<Func<GoodsAttributeModel, bool>> func = null)
        {
            return await DbContext.Select<GoodsAttributeModel>().WhereIf(func != null, func)
                .OrderByDescending(t => t.Sort)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}