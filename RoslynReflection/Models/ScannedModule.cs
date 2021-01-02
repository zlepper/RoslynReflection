using RoslynReflection.Collections;

namespace RoslynReflection.Models
{
    public record ScannedModule
    {
        public readonly ValueList<ScannedNamespace> Namespaces = new();

        internal void TrimEmptyNamespaces()
        {
            Namespaces.RemoveAll(ns => ns.Types.Count == 0);
        }
    }
}