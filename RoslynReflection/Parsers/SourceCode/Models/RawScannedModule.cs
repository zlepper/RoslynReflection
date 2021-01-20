using RoslynReflection.Collections;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record RawScannedModule
    {
        internal ValueList<RawScannedNamespace> Namespaces = new();
    }
}