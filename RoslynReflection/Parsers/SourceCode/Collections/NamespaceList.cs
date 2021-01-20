using System.Collections.Generic;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode.Collections
{
    /// <summary>
    ///     Ensures a namespace is only "created" once
    /// </summary>
    internal class NamespaceList
    {
        private readonly Dictionary<string, RawScannedNamespace> _namespaces = new();

        private readonly RawScannedModule _module;

        public NamespaceList(RawScannedModule module)
        {
            _module = module;
        }

        internal RawScannedNamespace GetNamespace(string name)
        {
            if (_namespaces.TryGetValue(name, out var existing)) return existing;

            var ns = _namespaces[name] = new RawScannedNamespace(name, _module);
            return ns;
        }
    }
}