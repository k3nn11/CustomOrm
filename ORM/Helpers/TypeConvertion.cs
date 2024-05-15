using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using ORM.Exceptions;

namespace ORM.Services
{
    public static class TypeConvertion
    {
        private static Dictionary<Type, SqlDbType> sqlTypeMap;

        private static Dictionary<Type, List<SqlDbType>> netTypeMap;

        static TypeConvertion()
        {
            SqlHelper();
            NetHelper();
        }

        public static SqlDbType GetDbType(Type netType)
        {
            netType = Nullable.GetUnderlyingType(netType) ?? netType;

           
            if (sqlTypeMap.TryGetValue(netType, out SqlDbType value))
            {
                return value;
            }

            throw new ArgumentException($"{netType.FullName} is not a supported .NET class");
        }

        public static SqlDbType GetDbType<T>()
        {
            return GetDbType(typeof(T));
        }

        public static Type GetNetType(SqlDbType sqlType)
        {
            foreach(var kvp in netTypeMap)
            {
                if(kvp.Value.Contains(sqlType))
                {
                    return kvp.Key;
                }
            }

            throw new ArgumentException($"{sqlType} is not a supported .NET class");
        }

        private static void SqlHelper()
        {
            sqlTypeMap = new Dictionary<Type, SqlDbType>
            {
                [typeof(string)] = SqlDbType.NVarChar,
                [typeof(char[])] = SqlDbType.NVarChar,
                [typeof(byte)] = SqlDbType.TinyInt,
                [typeof(short)] = SqlDbType.SmallInt,
                [typeof(int)] = SqlDbType.Int,
                [typeof(long)] = SqlDbType.BigInt,
                [typeof(byte[])] = SqlDbType.VarBinary,
                [typeof(bool)] = SqlDbType.Bit,
                [typeof(DateTime)] = SqlDbType.DateTime2,
                [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
                [typeof(decimal)] = SqlDbType.Decimal,
                [typeof(float)] = SqlDbType.Real,
                [typeof(double)] = SqlDbType.Float,
                [typeof(TimeSpan)] = SqlDbType.Time,
                [typeof(object)] = SqlDbType.Variant,
                [typeof(TimeSpan)] = SqlDbType.Time,
                [typeof(Guid)] = SqlDbType.UniqueIdentifier
            };
        }
        
        private static void NetHelper()
        {
            netTypeMap = new Dictionary<Type, List<SqlDbType>>
            {
                [typeof(string)] = [SqlDbType.NVarChar, SqlDbType.Char, SqlDbType.NChar, SqlDbType.NText, SqlDbType.Text, SqlDbType.VarChar],
                [typeof(byte)] = [SqlDbType.TinyInt],
                [typeof(short)] = [SqlDbType.SmallInt],
                [typeof(int)] = [SqlDbType.Int],
                [typeof(long)] = [SqlDbType.BigInt],
                [typeof(byte[])] = [SqlDbType.VarBinary, SqlDbType.Binary, SqlDbType.Timestamp],
                [typeof(bool)] = [SqlDbType.Bit],
                [typeof(DateTime)] = [SqlDbType.DateTime2, SqlDbType.Date, SqlDbType.DateTime, SqlDbType.SmallDateTime],
                [typeof(DateTimeOffset)] = [SqlDbType.DateTimeOffset],
                [typeof(decimal)] = [SqlDbType.Decimal, SqlDbType.Money, SqlDbType.SmallMoney],
                [typeof(float)] = [SqlDbType.Real],
                [typeof(double)] = [SqlDbType.Float],
                [typeof(TimeSpan)] = [SqlDbType.Time],
                [typeof(object)] = [SqlDbType.Variant],
                [typeof(TimeSpan)] = [SqlDbType.Time],
                [typeof(Guid)] = [SqlDbType.UniqueIdentifier]
            };
        }

        public static Type ToClrType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }
    }
}
