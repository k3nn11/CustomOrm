using ORM.Schema;

namespace ORM.Context
{
    public class DBContext
    {
        private DataBaseManager _dbManager;

        public DBContext(DataBaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        public IDbSet<T> Set<T>()
        {
            return (IDbSet<T>)_dbManager.GetTable(typeof(T));
        }
    }
}
