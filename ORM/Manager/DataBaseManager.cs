using ORM.ConnectionFactory;
using ORM.Manager;
using ORM.Extension;
using ORM.BaseClass;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ORM.Schema
{
    public class DataBaseManager
    {
        private readonly string _dBName;

        private Dictionary<Type, ITable> _tables;

        private Assembly _entityAssembly;

        public IProvider Provider { get; private set; }


        public DataBaseManager(string dBName, IProvider provider, Assembly entityAssembly)
        {
            _entityAssembly = entityAssembly;
            Provider = provider;
            _dBName = dBName;
            Provider.Connect();
            Build();
            CreateDatabase();
            CreateAllTables();
        }

        private void Build()
        {
            _tables = new Dictionary<Type, ITable>();
            var tabletypes = _entityAssembly.GetTypes().Where(x => x.HasBaseClass<IEntity>());
            foreach (var type in tabletypes)
            {
                Type genericType = typeof(TableManager<>).MakeGenericType([type]);
                ITable table = (ITable)Activator.CreateInstance(genericType, new object[] { this});
                _tables.Add(type, table);
            }
        }

        private void CreateAllTables()
        {
            foreach(var table in _tables.Values)
            {
                if (IsTableExist(table.Name))
                {
                    continue;
                }

                table.CreateTable();
            }
        }

        private void CreateDatabase()
        {
            if(IsDbExist(_dBName))
            {
                return;
            }

            string query = $"Create Database {_dBName}";

            try
            {
                Provider.NonQuery(query);    
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error details: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error details: {e.Message}");

            }
        }

        // Delete Table with key
        // Delete table generically also.
        public void DropTable(string tableName)
        {
            if(!_tables.Values.Any() || string.IsNullOrEmpty(tableName))
            {
                return;
            }

            var IsExist = _tables.Values.Select(x => x.Name == tableName).FirstOrDefault();

            if(IsExist)
            {
                var cmd = $"DROP TABLE {tableName}";
                Provider.NonQuery(cmd);
            }
        }

        public void Delete()
        {
            string cmd = $"DROP DATABASE {_dBName}";
           
            try
            {
                Provider.NonQuery(cmd);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public ITable? GetTable(Type type)
        {
            if (_tables.ContainsKey(type))
            {
                return _tables[type];
            }
            return null;
        }

        private bool IsDbExist(string name)
        {
            int index = 0;
            string query = "SELECT name from sys.databases";
            using (var cmd = Provider.CreateSqlCommand(query))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr[index].ToString() == name)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsTableExist(string tableName)
        {
            DataTable dTable = Provider.Connection.GetSchema("TABLES",
                               new string[] { null, null, tableName });
            return dTable.Rows.Count > 0;

        }
    }
}
