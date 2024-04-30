namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class PrimaryAttribute : Attribute
    {
    }
}
