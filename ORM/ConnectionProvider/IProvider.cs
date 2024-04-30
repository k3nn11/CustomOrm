using Microsoft.Data.SqlClient;

namespace ORM.ConnectionFactory
{
    public interface IProvider
    {
        char ParameterPrefix { get; }

        int NonQuery(string query);

        T Scalar<T>(string query);

        void Connect();

        SqlCommand CreateSqlCommand();
    }
}
