namespace ORM.CustomAttribute
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        public string TableName
        {
            get;
            private set;
        }
        public TableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
