using System;
using System.Linq;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal static class TypeExtensions
    {
        
        internal static bool IsRecord(this Type type)
        {
            // "Borrowed" from https://stackoverflow.com/a/64810188/3950006
            return type.GetMethods().Any(m => m.Name == "<Clone>$");
        }

        internal static string SafeFullname(this Type type)
        {
            if (type.FullName == null)
            {
                if (type.Namespace == null)
                {
                    return type.Name;
                }

                return $"{type.Namespace}.{type.Name}";
            }

            return type.FullName;
        }
    }
}