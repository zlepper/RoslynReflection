using System.Collections.Generic;
using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    /// <summary>
    ///     Ensures a namespace is only "created" once
    /// </summary>
    internal class SourceNamespaceList
    {
        private readonly Dictionary<string, ScannedNamespace> _namespaces = new();
        private readonly ScannedModule _sourceModule;

        public SourceNamespaceList(ScannedModule sourceModule)
        {
            _sourceModule = sourceModule;

            foreach (var ns in _sourceModule.Namespaces) _namespaces[ns.Name] = ns;
        }

        internal ScannedNamespace GetNamespace(string name)
        {
            if (_namespaces.TryGetValue(name, out var existing)) return existing;

            var ns = _namespaces[name] = new ScannedNamespace(_sourceModule, name);
            return ns;
        }
    }
}