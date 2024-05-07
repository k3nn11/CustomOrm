using Microsoft.Data.SqlClient;
using ORM.BaseClass;
using ORM.Context;
using ORM.CustomAttribute;
using ORM.Expressions;
using ORM.IO;
using ORM.Manager;
using ORM.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ORM.Schema
{
    public class TableManager<T> :IDbSet<T>, ITable where T : IEntity
    {
        public readonly Type _type = typeof(T);

        public PropertyInfo[] Properties { get; private set; }

        public PropertyInfo[] UpdateProperties {  get; private set; } 

        public PropertyInfo? PrimaryProperty { get; private set; }

        private DataTable DataTable { get; set; }

        private DataColumn DataColumn { get; set; }

        private TableReader<T> Reader { get; set; }

        private TableWriter<T> Writer { get; set; }

        public DataBaseManager Database {  get; private set; }

        public string Name { get; private set; }

        public TableManager(DataBaseManager dataBase)
        {
            Database = dataBase;
            Database.Provider.Connect();
            Reader = new TableReader<T>(this);
            Writer = new TableWriter<T>(this);

        }

        public void CreateTable()
        {
            string? command = CreateShemaScript();

            try
            {
                using (SqlCommand createCommand = Database.Provider.CreateSqlCommand())
                {
                    Database.Provider.NonQuery(command);
                }      
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error details {0}", e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error details {0}", e.Message);
            }
        }

        public void UpdateDbSchema()
        {
            string? command = UpdateSchemaScript();

            try
            {
                using (SqlCommand updateCommand = Database.Provider.CreateSqlCommand())
                {
                    Database.Provider.NonQuery(command);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error details {0}", e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public IEnumerable<T> Select()
        {
            return Reader.Read(string.Format(SQLQueries.SELECT, _type.Name));
        }

        public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Reader.Read(string.Format(SQLQueries.SELECT, Name) + string.Format(SQLQueries.WHERE_CLAUSE, builder.WhereClause));
        }

        public void Insert(T entity)
        {
            Writer.Insert([entity]);
        }

        public void Insert(IEnumerable<T> entities)
        {
            Writer.Insert(entities);
        }

        public int Update(T entity)
        {
            return Writer.Update(new[] { entity });
        }

        public int Update(IEnumerable<T> entities)
        {
            return Writer.Update(entities);
        }

        public int Delete(T entity)
        {
            var primaryKey = PrimaryProperty.GetValue(entity);
            string query = string.Format(SQLQueries.DELETE, _type.Name) + string.Format(SQLQueries.WHERE_CLAUSE, PrimaryProperty.Name + "=" + primaryKey);
            return Database.Provider.NonQuery(query);
        }

        public int Delete(Expression<Func<T,bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            string query = string.Format(SQLQueries.DELETE, _type.Name) + string.Format(SQLQueries.WHERE_CLAUSE, builder.WhereClause);
            return Database.Provider.NonQuery(query);
        }

        public int Delete(IEnumerable<T> entities)
        {
            return Writer.Delete(entities);
        }

        public int DeleteAll()
        {
            return Database.Provider.NonQuery(string.Format(SQLQueries.DELETE, _type.Name));
        }

        private string? CreateShemaScript()
        {
            DataTable? dataTable = Build();
            if (dataTable == null)
            {
                return null;
            }
            StringBuilder sqlScript = new();
            sqlScript.AppendFormat("CREATE TABLE [{0}] (", dataTable.TableName);

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sqlScript.AppendFormat("\n\t[{0}]", dataTable.Columns[i].ColumnName);
                sqlScript.Append(TypeConvertion.ToSqlDbType(dataTable.Columns[i].DataType).ToString());

                if (dataTable.Columns[i].AutoIncrement)
                {
                    sqlScript.AppendFormat(" IDENTITY({0},{1})",
                    dataTable.Columns[i].AutoIncrementSeed = 1,
                    dataTable.Columns[i].AutoIncrementStep);
                }

                if (dataTable.Columns[i].AllowDBNull)
                {
                    sqlScript.Append(" NOT NULL");
                }

                sqlScript.Append(',');

            }
            if (dataTable.PrimaryKey.Length > 0)
            {
                StringBuilder primaryKeySql = new StringBuilder();

                primaryKeySql.AppendFormat("\n\tCONSTRAINT PK_{0} PRIMARY KEY (", dataTable.TableName);

                for (int j = 0; j < dataTable.PrimaryKey.Length; j++)
                {
                    primaryKeySql.AppendFormat("{0},", dataTable.PrimaryKey[j].ColumnName);
                }

                primaryKeySql.Remove(primaryKeySql.Length - 1, 1);
                primaryKeySql.Append(')');

                sqlScript.Append(primaryKeySql);
            }
            else
            {
                sqlScript.Remove(sqlScript.Length - 1, 1);
            }

            sqlScript.Append("\n);");
            return sqlScript.ToString();
        }

       // to do 
       // rewrite naming of method.
       // Forget implementation word
        private string UpdateSchemaScript()
        {
            string tableName = _type.Name;
            StringBuilder sqlScript = new();
            List<string?> dbColumns = GetDbColumns(tableName);
            DataColumnCollection? modelColumns = Build().Columns;
            var currentIndex = 0;
            var newIndex = 0;

            if (dbColumns == null || dbColumns.Count == 0)
            {
                CreateTable();
            }
            while (newIndex < modelColumns.Count && currentIndex < dbColumns.Count)
            {
                if (dbColumns[currentIndex] == modelColumns[newIndex].ColumnName)
                {
                    currentIndex++;
                    newIndex++;
                }
                else
                {
                    if (currentIndex + 1 < dbColumns.Count && dbColumns[currentIndex + 1] == modelColumns[newIndex].ColumnName)
                    {
                        sqlScript.AppendFormat("ALTER TABLE {0} DROP COLUMN {1};\n", tableName, dbColumns[currentIndex]);
                        dbColumns.RemoveAt(currentIndex);
                    }
                    else if (newIndex + 1 < modelColumns.Count && dbColumns[currentIndex] == modelColumns[newIndex + 1].ColumnName)
                    {
                        var type = modelColumns[newIndex].GetType();
                        sqlScript.AppendFormat("ALTER TABLE {0} ADD {1} ", tableName, modelColumns[newIndex]);
                        sqlScript.Append(TypeConvertion.ToSqlDbType(type).ToString());
                        sqlScript.Append(';');
                        dbColumns.Insert(currentIndex, modelColumns[newIndex].ToString());
                        currentIndex++;
                    }
                    else
                    {
                        var type = modelColumns[newIndex].GetType();
                        sqlScript.AppendFormat("ALTER TABLE {0} ADD COLUMN {1} ", tableName, modelColumns[newIndex]);
                        sqlScript.Append(TypeConvertion.ToSqlDbType(type).ToString());
                        sqlScript.Append(';');
                        dbColumns[currentIndex] = modelColumns[newIndex].ToString();
                        currentIndex++;
                    }
                    newIndex++;
                }               
            }

            while (newIndex < modelColumns.Count)
            {
                var type = modelColumns[newIndex].GetType();
                sqlScript.AppendFormat("ALTER TABLE {0} ADD COLUMN {1} {2};\n", tableName, modelColumns[newIndex] ,TypeConvertion.ToSqlDbType(type).ToString());
                dbColumns.Add(modelColumns[newIndex].ColumnName);
                newIndex++;
            }
            return sqlScript.ToString();
        }
        // to do 
        // make method shorter.
        private DataTable? Build()
        {
            DataTable = new();
            var tableAttribute = (TableAttribute?)Attribute.GetCustomAttribute(_type, typeof(TableAttribute));
             Properties = _type.GetProperties();
            UpdateProperties = _type.GetProperties().Where(x => Attribute.GetCustomAttribute(x, typeof(PrimaryAttribute)) == null).ToArray();

            if (tableAttribute is not null)
            {
                DataTable.TableName = tableAttribute.TableName;
            }
            else
            {
                DataTable.TableName = _type.Name;
            }

            Name = DataTable.TableName;

            foreach (var prop in Properties)
            {
                
                DataColumn = new();
                DataTable.Columns.Add(DataColumn);

                Attribute[] attributes = Attribute.GetCustomAttributes(prop);
                AssignDataColumns(prop, attributes);
            }
            return DataTable;
        }

        private void AssignDataColumns(PropertyInfo prop, Attribute[] attributes)
        {
            if (attributes.Length > 0)
            {
                foreach (Attribute attr in attributes)
                {
                    if (attr is ColumnAttribute columnAttr)
                    {
                        DataColumn.ColumnName = columnAttr.ColumnName;
                        DataColumn.DataType = columnAttr.DataType;
                        DataColumn.AutoIncrement = columnAttr.AutoIncrement;
                        DataColumn.AllowDBNull = columnAttr.AllowNullable;

                        if(columnAttr.MaxLength != 0)
                        {
                            DataColumn.MaxLength = columnAttr.MaxLength;
                        }
                    }

                    if (attr is PrimaryAttribute pk)
                    {
                        DataTable.PrimaryKey = [DataColumn];
                        DataColumn.DataType = prop.PropertyType;
                        DataColumn.ColumnName = prop.Name;
                        DataColumn.DataType = prop.PropertyType;
                        DataTable.PrimaryKey = [DataColumn];
                        PrimaryProperty = prop;
                    }
                    //if(attr is ForeignKeyAttribute foreign)
                    //{
                    //    ForeignKeyConstraint keyConstraint = new("FK_" + dColumn.ColumnName,  )
                    //    dtable.Constraints.Add()
                    //}    
                }
            }
            else
            {
                string pattern = @"\w+Id\b";
                Regex rg = new(pattern, RegexOptions.IgnoreCase);
                if (rg.IsMatch(prop.Name))
                {
                    DataTable.PrimaryKey = [DataColumn];
                    DataColumn.ColumnName = prop.Name;
                    DataColumn.DataType = prop.PropertyType;
                }
                else
                {
                    DataColumn.ColumnName = prop.Name;
                    DataColumn.DataType = prop.PropertyType;
                    DataColumn.AutoIncrement = false;
                    DataColumn.AllowDBNull = Nullable.GetUnderlyingType(prop.PropertyType) != null;
                }
            }
        }

        private List<string?> GetDbColumns(string tableName)
        {
            List<string?> columns = [];
            DataTable? dataTable = new();
            string query = $"SELECT COLUMN_NAME\n\tFROM INFORMATION_SCHEMA.COLUMNS\n\tWHERE TABLE_NAME = '{tableName}'";
            using (SqlCommand cmd = Database.Provider.CreateSqlCommand())
            {
                cmd.CommandText = query;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i].ItemArray.GetValue(0);
                columns.Add(row.ToString());
            }
            return columns;
        }

        //private bool IsTableExist()
        //{
        //    string tableName = _type.Name;
        //    using (_connection)
        //    {
        //        _connection.Open();
        //        DataTable dTable = _connection.GetSchema("TABLES",
        //                       new string[] { null, null, tableName });
        //        return dTable.Rows.Count > 0;
        //    }
        //}
    }
}
