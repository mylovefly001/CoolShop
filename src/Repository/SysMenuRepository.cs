using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Extend;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class SysMenuRepository : BaseRepository
    {
        public SysMenuRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> Delete(dynamic[] ids)
        {
            return await DbContext.Delete<SysMenuModel>(ids).ExecuteAffrowsAsync();
        }

        public async Task<int> Delete(int id = 0)
        {
            return await DbContext.Delete<SysMenuModel>(id).ExecuteAffrowsAsync();
        }


        public async Task<long> Insert(SysMenuModel model)
        {
            return await DbContext.Insert<SysMenuModel>().AppendData(model).ExecuteIdentityAsync();
        }

        public async Task<int> Update(SysMenuModel model)
        {
            return await DbContext.Update<SysMenuModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        public async Task<SysMenuModel> GetInfo(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await DbContext.Select<SysMenuModel>().WhereIf(func != null, func).ToOneAsync();
        }

        public async Task<SysMenuModel> GetInfo(int id = 0)
        {
            return await DbContext.Select<SysMenuModel>().Where(t => t.Id == id).ToOneAsync();
        }

        public async Task<List<SysMenuModel>> GetList(Expression<Func<SysMenuModel, bool>> func = null)
        {
            return await DbContext.Select<SysMenuModel>().WhereIf(func != null, func).OrderByDescending(t => t.Sort & t.Id).ToListAsync();
        }
    }
}