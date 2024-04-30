using System.Globalization;

namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PrimaryAttribute : Attribute
    {
        public  string Name {  get;}

        public PrimaryAttribute(string name) 
        {
            Name = name;
        }
    }
}
