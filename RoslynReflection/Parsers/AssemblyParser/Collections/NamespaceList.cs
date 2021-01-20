using System.Collections.Generic;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser.Collections
{
    public class NamespaceList
    {
        private readonly Dictionary<string, ScannedNamespace> _namespaces = new();
        private readonly ScannedModule _module;


        public NamespaceList(ScannedModule module)
        {
            _module = module;
        }

        internal ScannedNamespace GetNamespace(string name)
        {
            if (_namespaces.TryGetValue(name, out var existing)) return existing;

            var ns = _namespaces[name] = new ScannedNamespace(_module, name);
            return ns;
        }
    }
}