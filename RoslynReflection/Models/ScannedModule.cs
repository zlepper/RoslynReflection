using RoslynReflection.Collections;

namespace RoslynReflection.Models
{
    public record ScannedModule
    {
        public readonly ValueList<ScannedNamespace> Namespaces = new();
    }
}