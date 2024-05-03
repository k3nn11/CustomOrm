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

        public IProvider Provider {  get; private set; }


        public DataBaseManager(string dBName, IProvider provider, Assembly entityAssembly)
        {
            _entityAssembly = entityAssembly;
            Provider = provider;
            _dBName = dBName;
            Provider.Connect();
            Build();
            CreateAllTables();
        }

        private void Build()
        {
            _tables = new Dictionary<Type, ITable>();
            var tabletypes = _entityAssembly.GetTypes().Where(x => x.HasBaseClass<IEntity>());
            foreach (var type in tabletypes)
            {
                Type genericType = typeof(TableManager<>).MakeGenericType([type]);
                ITable table = (ITable)Activator.CreateInstance(genericType, new object[] { this, type.Name });
                _tables.Add(type, table);
            }
        }

        private void CreateAllTables()
        {
            CreateDatabase();
            foreach(var table in _tables.Values)
            {
                table.CreateTable();
            }
        }

        private int CreateDatabase()
        {
            string query = $"Create Database {_dBName}";
            int check = 0;
            try
            {
                check = Provider.NonQuery(query);    
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error details: {e.Message}");
            }
            return check;
        }

        public void Delete()
        {
            string cmd = $"DROP DATABASE {_dBName}";
           
            try
            {
               using (SqlCommand command = Provider.CreateSqlCommand() )
                {
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
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
    }
}
