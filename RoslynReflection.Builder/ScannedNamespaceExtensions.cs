using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser;

namespace RoslynReflection.Builder
{
    public static class ScannedNamespaceExtensions
    {
        public static ScannedType AddClass(this ScannedNamespace ns, string name)
        {
            return new(name, ns, null)
            {
                IsClass = true
            };
        }

        public static ScannedType AddInterface(this ScannedNamespace ns, string name)
        {
            return new(name, ns, null)
            {
                IsInterface = true
            };
        }

        public static ScannedType AddRecord(this ScannedNamespace ns, string name)
        {
            return new(name, ns, null)
            {
                IsClass = true,
                IsRecord = true
            };
        }
    }
}