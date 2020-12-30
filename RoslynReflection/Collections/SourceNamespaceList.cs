using System.Collections;
using System.Collections.Generic;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Collections
{
    /// <summary>
    /// Ensures a namespace is only "created" once
    /// </summary>
    internal class SourceNamespaceList
    {
        private Dictionary<string, SourceNamespace> _namespaces = new();
        private SourceModule _sourceModule;
        
        public SourceNamespaceList(SourceModule sourceModule)
        {
            _sourceModule = sourceModule;

            foreach (var ns in _sourceModule.SourceNamespaces)
            {
                _namespaces[ns.Name] = ns;
            }
        }

        internal SourceNamespace GetNamespace(string name)
        {
            if (_namespaces.TryGetValue(name, out var existing))
            {
                return existing;
            }

            var ns = _namespaces[name] = new SourceNamespace(_sourceModule, name);
            _sourceModule.SourceNamespaces.Add(ns);
            return ns;
        }
    }
}