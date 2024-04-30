using Microsoft.Data.SqlClient;
using System.Data;
using ORM.Exceptions;
using ORM.DataContextImplementation;

namespace ORM.ConnectionFactory
{
    // to do
    // pass parameter in createSqlCommand for query.
    // process nonquery method naming.
    // 
    public class SqlProvider : IProvider
    {
        public char ParameterPrefix => '@';

        public SqlConnection Connection { get; set; }

        public DataContext DataContext { get; set; }

        public SqlProvider(DataContext dataContext)
        {
            DataContext = dataContext;
            Connection = DataContext.Connection;
        }

        public SqlCommand CreateSqlCommand()
        {
            return new SqlCommand(string.Empty, Connection);
        }

        public int NonQuery(string query)
        {
            using (var command = new SqlCommand(query, Connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public T Scalar<T>(string query)
        {
            using (var command = new SqlCommand(query, Connection))
            {
                return (T)command.ExecuteScalar();
            }
        }

        public void Connect()
        {
            if (Connection != null)
            {
                throw new InvalidConnectionException("Connection is not available");
            }
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }
    }
}
