using CoolShop.Core.Extend;

namespace CoolShop.Repository
{
    public class BaseRepository
    {
        protected IFreeSql DbContext { get; }

        protected BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext.Mysql;
        }
    }
}