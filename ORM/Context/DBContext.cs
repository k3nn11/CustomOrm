using Microsoft.Data.SqlClient;
using ORM.BaseClass;
using System;


namespace ORM.Context
{
    public class DBContext<T> : IDisposable where T : IEntity
    {
        private bool _IsDisposed;

        public DBContext(IDbSet<T> _DbSet, string connection)
        {
            DbSet = _DbSet;
            Connection = new SqlConnection(connection);
        }

        public IDbSet<T> DbSet { get; private set; }

        public SqlConnection Connection { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }
                _IsDisposed = true;
            }
        }
    }
}
