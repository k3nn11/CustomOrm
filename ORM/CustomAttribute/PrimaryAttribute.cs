namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PrimaryAttribute : Attribute
    {
        public bool IsAutoIncrement { get; }
        public PrimaryAttribute() 
        { 
            IsAutoIncrement = true;
        }
    }
}
