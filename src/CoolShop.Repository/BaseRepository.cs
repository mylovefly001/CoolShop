using CoolShop.Core.Library;

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