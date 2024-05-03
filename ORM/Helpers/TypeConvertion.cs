using System.Data;
using Microsoft.Data.SqlClient;
using ORM.Exceptions;

namespace ORM.Services
{
    public static class TypeConvertion
    {
        public static SqlDbType ToSqlDbType(Type type)
        {
            DbType dbType = ToDbType(type);
            SqlParameter parameter = new();
            try
            {
                parameter.DbType = dbType;
            }
            catch (InvalidMappingException e)
            {

                Console.WriteLine($"Error details: {e.Message}");
            }
            SqlDbType sqlDbType = parameter.SqlDbType;
            return sqlDbType;
        }
        private static DbType ToDbType(Type pType)
        {
            switch (pType.Name.ToLower())
            {
                case "byte":
                    return DbType.Byte;
                case "sbyte":
                    return DbType.SByte;
                case "short":
                case "int16":
                    return DbType.Int16;
                case "uint16":
                    return DbType.UInt16;
                case "int32":
                    return DbType.Int32;
                case "uint32":
                    return DbType.UInt32;
                case "int64":
                    return DbType.Int64;
                case "uint64":
                    return DbType.UInt64;
                case "single":
                    return DbType.Single;
                case "double":
                    return DbType.Double;
                case "decimal":
                    return DbType.Decimal;
                case "bool":
                case "boolean":
                    return DbType.Boolean;
                case "string":
                    return DbType.String;
                case "char":
                    return DbType.StringFixedLength;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "DateTimeOffset":
                    return DbType.DateTimeOffset;
                case "byte[]":
                    return DbType.Binary;
                case "byte?":
                    return DbType.Byte;
                case "sbyte?":
                    return DbType.SByte;
                case "short?":
                    return DbType.Int16;
                case "ushort?":
                    return DbType.UInt16;
                case "int?":
                    return DbType.Int32;
                case "uint?":
                    return DbType.UInt32;
                case "long?":
                    return DbType.Int64;
                case "ulong?":
                    return DbType.UInt64;
                case "float?":
                    return DbType.Single;
                case "double?":
                    return DbType.Double;
                case "decimal?":
                    return DbType.Decimal;
                case "bool?":
                    return DbType.Boolean;
                case "char?":
                    return DbType.StringFixedLength;
                case "Guid?":
                    return DbType.Guid;
                case "DateTime?":
                    return DbType.DateTime;
                case "DateTimeOffset?":
                    return DbType.DateTimeOffset;
                default:
                    return DbType.String;
            }
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
