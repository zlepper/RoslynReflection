using System;

namespace RoslynReflection.Extensions
{
    internal static class TypeExtensions
    {
        internal static string GetNonGenericTypeName(this Type type)
        {
            var name = type.Name;
            if (name.Contains("`"))
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            return name;
        } 
    }
}