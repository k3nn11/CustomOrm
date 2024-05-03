using Microsoft.Data.SqlClient;
using ORM.BaseClass;
using ORM.Manager;
using ORM.Schema;

namespace ORM.DataContextImplementation
{

    public class DataContext<T> : IDisposable where T : IEntity
    {
        private bool isDisposed;

        private ITableIO<T> _tableIO;

        public SqlConnection Connection {  get; set; }

        public DataContext(string connection)
        {
            Connection = new SqlConnection(connection);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed) 
            { 
                if(disposing)
                {
                    Connection.Dispose();
                }
                isDisposed = true;
            }
        }
    }
}
