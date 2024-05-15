using ORM.BaseClass;

namespace ORM.Context
{
    public class DBContext<T>
    {
        public DBContext(IDbSet<T> _DbSet)
        {
            DbSet = _DbSet;
        }

        public IDbSet<T> DbSet { get; private set; }
    }
}
