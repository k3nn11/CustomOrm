using Microsoft.Data.SqlClient;
using ORM.BaseClass;

namespace ORM.Context
{
    public class DBContext<T>  where T : IEntity
    {
        private bool _IsDisposed;

        public DBContext(IDbSet<T> _DbSet)
        {
            DbSet = _DbSet;
            //Connection = new SqlConnection(connection);
        }

        public IDbSet<T> DbSet { get; private set; }

        //public SqlConnection Connection { get; private set; }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_IsDisposed)
        //    {
        //        if (disposing)
        //        {
        //            Connection.Dispose();
        //        }
        //        _IsDisposed = true;
        //    }
        //}
    }
}
