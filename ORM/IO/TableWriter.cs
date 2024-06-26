﻿using Microsoft.Data.SqlClient;
using ORM.Exceptions;
using ORM.Schema;
using ORM.Services;
using System.Text;

namespace ORM.IO
{
    public class TableWriter<T>
    {

        public TableWriter(TableManager<T> tableManager)
        {
            TableManager = tableManager;
        }

        private TableManager<T> TableManager { get; }

        public int Insert(IEnumerable<T> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }

            StringBuilder queryContent = new StringBuilder();

            using (SqlCommand command = TableManager.Database.Provider.CreateSqlCommand())
            {

                int id = 0;

                foreach (var entity in entities)
                {
                    queryContent.Append('(');

                    string prefix = "";

                    foreach (var property in TableManager.UpdateProperties)
                    {
                        queryContent.Append(prefix);
                        prefix = ",";

                        object? value = property.GetValue(entity);

                        if (value is byte[])
                        {
                            queryContent.Append(string.Format(TableManager.Database.Provider.ParameterPrefix + "{0}{1}", property.Name, id));
                            string name = TableManager.Database.Provider.ParameterPrefix + property.Name + id;
                            SqlParameter parameter = new();
                            {
                                parameter.ParameterName = name;
                                parameter.Value = value;
                            }
                            command.Parameters.Add(parameter);
                        }
                        else
                        {
                            queryContent.Append("'" + value.ToString() + "'");
                        }
                    }

                    queryContent.Append(')');
                    queryContent.Append(',');

                    id++;
                }

                queryContent = queryContent.Remove(queryContent.Length - 1, 1);

                var properties = string.Join(',', TableManager.UpdateProperties.Select(x => x.Name));
                command.CommandText = string.Format(SQLQueries.INSERT, TableManager.Name, properties, string.Format("{0}", queryContent.ToString()));
                return TableManager.Database.Provider.NonQuery(command.CommandText);
            }
        }
        // to do 
        // refactor to shorten method
        public int Update(IEnumerable<T> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }
            if (TableManager.UpdateProperties.Length == 0)
            {
                throw new InvalidMappingException("Unable to update elements. " +TableManager.Name + " has no update property.");
            }

            using (SqlCommand command = TableManager.Database.Provider.CreateSqlCommand())
            {
                int i = 0;

                foreach (var entity in entities)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var property in TableManager.UpdateProperties)
                    {
                        sb.Append(string.Format("{0} = {2}{0}{1},", property.Name, i, TableManager.Database.Provider.ParameterPrefix));
                        SqlParameter parameter = new(TableManager.Database.Provider.ParameterPrefix + property.Name + i,
                            property.GetValue(entity));

                        command.Parameters.Add(parameter);
                    }

                    sb = sb.Remove(sb.Length - 1, 1);

                    var text = string.Format("{0}", string.Join(",", sb.ToString()));

                    Object? primary = TableManager.PrimaryProperty.GetValue(entity);
                    command.CommandText += string.Format(SQLQueries.UPDATE, TableManager.Name, text, TableManager.PrimaryProperty.Name, primary.ToString()) + ";";

                    i++;
                }
                
                return TableManager.Database.Provider.NonQuery(command.CommandText);
            }

        }

        public int Delete(IEnumerable<T> entities)
        {
            var uids = entities.Select(x => TableManager.PrimaryProperty.GetValue(x)).ToArray();
            string queryContent = string.Join(",", uids);
            return TableManager.Database.Provider.NonQuery(string.Format(SQLQueries.DELETE_IN, TableManager.Name, queryContent));
        }
    }
}
