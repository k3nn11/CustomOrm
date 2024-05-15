using Microsoft.Data.SqlClient;
using System.Data;
using ORM.Exceptions;
using ORM.Context;

namespace ORM.ConnectionFactory
{
    // to do
    // pass parameter in createSqlCommand for query.
    // process nonquery method naming.
    // 
    public class SqlProvider : IProvider
    {
        public char ParameterPrefix => '@';

        public SqlConnection Connection { get; private set; }


        public SqlProvider(string connection)
        {
            Connection = new SqlConnection(connection);
        }

        public SqlCommand CreateSqlCommand()
        {
            return new SqlCommand(string.Empty, Connection);
        }

        public SqlCommand CreateSqlCommand(string command)
        {
            return new SqlCommand(command, Connection);
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
            if (Connection == null)
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
