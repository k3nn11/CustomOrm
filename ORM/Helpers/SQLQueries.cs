
namespace ORM.Services
{
    // to do 
     // rename the folder from services to Helpers.
    public class SQLQueries
    {
        public const string COUNT = "SELECT COUNT(*) FROM [{0}]";

        public const string SELECT = "SELECT * FROM [{0}]";

        public const string WHERE_CLAUSE = " where {0}";

        public const string DROP = "DROP TABLE IF EXISTS {0}";

        public const string DELETE = "DELETE FROM {0}";

        public const string DELETE_IN = "DELETE FROM {0} WHERE id IN({1})";

        public const string DELETE_WHERE = "DELETE FROM {0} WHERE {1}";

        public const string INSERT = "INSERT INTO [{0}] ({1}) VALUES {2}";

        public const string UPDATE = "UPDATE `{0}` SET {1} WHERE {2} = {3}";

        public const string MAX = "SELECT MAX(`{1}`) FROM `{0}`";

        public const string CREATE_TABLE = "CREATE TABLE [{0}]";

        public const string IDENTITY = "IDENTITY({0},{1})";

        public const string NOT_NULL = "NOT NULL";

        public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public const string ADDCOLUMN = "ALTER TABLE {0} ADD COLUMN {1}";

        public const string DROPCOLUMN = "ALTER TABLE {0} DROP COLUMN {1}";

        public const string IDENTITY_INSERT_ON = "SET IDENTITY_INSERT [{0}] ON";

        public const string IDENTITY_INSERT_OFF = "SET IDENTITY_INSERT [{0}] OFF";

        public const string PK_CONSTRAINT = "CONSTRAINT PK_{0} PRIMARY KEY";
    }
}
