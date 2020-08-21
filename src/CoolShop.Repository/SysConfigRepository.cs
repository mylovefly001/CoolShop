using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolShop.Core.Library;
using CoolShop.Model;

namespace CoolShop.Repository
{
    public class SysConfigRepository : BaseRepository
    {
        public SysConfigRepository(DbContext dbContext) : base(dbContext)
        {
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Update(SysConfigModel model)
        {
            return await DbContext.Update<SysConfigModel>().SetSource(model).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <returns></returns>
        public async Task<SysConfigModel> GetInfo()
        {
            return await DbContext.Select<SysConfigModel>().ToOneAsync();
        }
    }
}