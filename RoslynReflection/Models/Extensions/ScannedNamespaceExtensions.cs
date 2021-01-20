using System.Collections.Generic;

namespace RoslynReflection.Models.Extensions
{
    public static class ScannedNamespaceExtensions
    {
        internal static void AddTypes(this ScannedNamespace ns, IEnumerable<ScannedType> types)
        {
            foreach (var type in types)
            {
                ns.AddType(type);
            }
        }
        
        internal static string NameAsPrefix(this ScannedNamespace ns)
        {
            if (ns.Name == "")
            {
                return "";
            }

            return ns.Name + ".";
        }
    }
}