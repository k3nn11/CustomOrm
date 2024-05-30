using ORM.Exceptions;
using ORM.Schema;
using ORM.Services;
using Microsoft.Data.SqlClient;

namespace ORM.IO
{
    public class TableReader<T>
    {
        public TableReader(TableManager<T> tableManager) 
        { 
            TableManager = tableManager;
        }

        private TableManager<T> TableManager { get; }

        // to do 
        // check on query parameter and refactor to use the Provider
        public IEnumerable<T> Read(string query)
        {
            List<T> results = [];

            using (SqlCommand command = TableManager.Database.Provider.CreateSqlCommand())
            {
                command.CommandText = query;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.FieldCount != TableManager.Properties.Length)
                    {
                        throw new InvalidMappingException("The mapping of table '" + TableManager.Name + "' is not consistent with the SQL structure.");
                    }
                    while (reader.Read())
                    {
                        var obj = new object[TableManager.Properties.Length];

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var type = reader[i];
                            obj[i] =  reader[i];
                        }

                        T entity = Activator.CreateInstance<T>();

                        for (int i = 0; i < TableManager.Properties.Length; i++)
                        {
                            TableManager.Properties[i].SetValue(entity, obj[i]);
                        }

                        results.Add(entity);
                   
                    }
                }
            }

            return results;
        }
    }
}
