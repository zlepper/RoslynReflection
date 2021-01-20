using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Test.TestHelpers.Extensions
{
    internal static class RawScannedModuleExtensions
    {
        internal static RawScannedNamespace AddNamespace(this RawScannedModule module, string name)
        {
            return new(name, module);
        }
    }
}