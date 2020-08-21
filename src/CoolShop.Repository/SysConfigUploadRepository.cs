using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class SysConfigUploadRepository : BaseRepository
    {
        public SysConfigUploadRepository(DbContext dbContext) : base(dbContext)
        {
        }


        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<SysConfigUploadModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(SysConfigUploadModel model)
        {
            return await DbContext.Insert<SysConfigUploadModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(SysConfigUploadModel model)
        {
            return await DbContext.Update<SysConfigUploadModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysConfigUploadModel> GetInfo(Expression<Func<SysConfigUploadModel, bool>> func = null)
        {
            return await DbContext.Select<SysConfigUploadModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysConfigUploadModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<SysConfigUploadModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysConfigUploadModel>> GetList(Expression<Func<SysConfigUploadModel, bool>> func = null)
        {
            return await DbContext.Select<SysConfigUploadModel>().WhereIf(func != null, func)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}