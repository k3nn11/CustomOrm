namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public  string ColumnName { get;} 

        public bool AllowNullable { get; set; }

        public bool AutoIncrement { get; set; }

        public Type DataType { get;}

        public ColumnAttribute(string columnName, Type dataType)
        {
            AllowNullable = false;
            AutoIncrement = false;
            ColumnName = columnName;
            DataType = dataType;
        }
    }
}
