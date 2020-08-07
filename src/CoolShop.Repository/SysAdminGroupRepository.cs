using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class SysAdminGroupRepository : BaseRepository
    {
        public SysAdminGroupRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据ID删除一批数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> Delete(List<int> ids)
        {
            return await DbContext.Delete<SysAdminGroupModel>(ids).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<SysAdminGroupModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(SysAdminGroupModel model)
        {
            return await DbContext.Insert<SysAdminGroupModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(SysAdminGroupModel model)
        {
            return await DbContext.Update<SysAdminGroupModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysAdminGroupModel> GetInfo(Expression<Func<SysAdminGroupModel, bool>> func = null)
        {
            return await DbContext.Select<SysAdminGroupModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysAdminGroupModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<SysAdminGroupModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysAdminGroupModel>> GetList(Expression<Func<SysAdminGroupModel, bool>> func = null)
        {
            return await DbContext.Select<SysAdminGroupModel>().WhereIf(func != null, func)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}