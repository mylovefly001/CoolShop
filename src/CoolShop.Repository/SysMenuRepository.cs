using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class SysMenuRepository : BaseRepository
    {
        public SysMenuRepository(DbContext dbContext) : base(dbContext)
        {
        }

       /// <summary>
       /// 根据ID删除一批数据
       /// </summary>
       /// <param name="ids"></param>
       /// <returns></returns>
        public async Task<int> Delete(List<int> ids)
        {
            return await DbContext.Delete<SysMenuModel>(ids).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<SysMenuModel>(id).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> Insert(SysMenuModel model)
        {
            return await DbContext.Insert<SysMenuModel>().AppendData(model).ExecuteIdentityAsync();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(SysMenuModel model)
        {
            return await DbContext.Update<SysMenuModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<SysMenuModel> GetInfo(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await DbContext.Select<SysMenuModel>().WhereIf(func != null, func).ToOneAsync();
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysMenuModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<SysMenuModel>().Where(t => t.Id == id).ToOneAsync();
        }

        /// <summary>
        /// 根据条件获取一批数据
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<List<SysMenuModel>> GetList(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await DbContext.Select<SysMenuModel>().WhereIf(func != null, func)
                .OrderByDescending(t => t.Sort)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}