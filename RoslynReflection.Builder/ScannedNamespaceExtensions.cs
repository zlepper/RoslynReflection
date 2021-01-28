using System;
using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser;

namespace RoslynReflection.Builder
{
    public static class ScannedNamespaceExtensions
    {
        private static ScannedType AddType(ScannedNamespace ns, string name, Action<ScannedType> modify)
        {
            var type = new ScannedType(name, ns, null);
            ns.AddType(type);
            
            modify(type);

            return type;
        }
        
        public static ScannedType AddClass(this ScannedNamespace ns, string name)
        {
            return AddType(ns, name, t => t.IsClass = true);
        }

        public static ScannedType AddInterface(this ScannedNamespace ns, string name)
        {
            return AddType(ns, name, t => t.IsInterface = true);
        }

        public static ScannedType AddRecord(this ScannedNamespace ns, string name)
        {
            return AddType(ns, name, t => t.IsClass = t.IsRecord = true);
        }
    }
}