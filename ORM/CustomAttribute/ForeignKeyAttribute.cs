namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute()
        {
        }
    }
}
