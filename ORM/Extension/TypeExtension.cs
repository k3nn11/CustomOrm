using System.Reflection;

namespace ORM.Extension
{
    public static class TypeExtension
    {
        public static bool HasBaseClass<T>(this Type type) where T : class
        {
            return type.GetInterfaces().Any(x => x == typeof(T));
        }

        public static bool HasAttribute<T>(this PropertyInfo property)
        {
            return property.CustomAttributes.Any(x => x.AttributeType == typeof(T));
        }
    }
}