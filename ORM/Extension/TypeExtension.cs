using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Extension
{
    public static class TypeExtension
    {
        public static bool HasBaseClass<T>(this Type type) where T : class
        {
            return type.GetInterfaces().Any(x => x == typeof(T));
        }
    }
}
