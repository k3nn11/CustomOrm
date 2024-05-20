namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; set; } 

        public bool AllowNullable { get; set; }

        public Type DataType { get; set; }

        public int MaxLength {  get; set; }

        public ColumnAttribute(string columnName, Type dataType)
        {
            AllowNullable = false;
            ColumnName = columnName;
            DataType = dataType;
        }
    }
}
