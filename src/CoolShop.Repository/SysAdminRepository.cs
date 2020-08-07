using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class SysAdminRepository : BaseRepository
    {
        public SysAdminRepository(DbContext dbContext) : base(dbContext)
        {
        }
        
         /// <summary>
        /// 根据ID删除一批数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> Delete(List<int> ids)
        {
            return await DbContext.Delete<SysAdminModel>(ids).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<SysAdminModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(SysAdminModel model)
        {
            return await DbContext.Insert<SysAdminModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(SysAdminModel model)
        {
            return await DbContext.Update<SysAdminModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysAdminModel> GetInfo(Expression<Func<SysAdminModel, bool>> func = null)
        {
            return await DbContext.Select<SysAdminModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysAdminModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<SysAdminModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysAdminModel>> GetList(Expression<Func<SysAdminModel, bool>> func = null)
        {
            return await DbContext.Select<SysAdminModel>().WhereIf(func != null, func)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}