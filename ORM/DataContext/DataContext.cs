using Microsoft.Data.SqlClient;
using ORM.BaseClass;
using ORM.Schema;

namespace ORM.DataContextImplementation
{

    public class DataContext : IDisposable
    {
        public SqlConnection Connection {  get; set; }

        public DataContext(string connection)
        {
            Connection = new SqlConnection(connection);
            Connection.Open();
        }

      
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
