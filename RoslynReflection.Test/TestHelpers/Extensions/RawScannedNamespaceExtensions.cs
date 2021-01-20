using RoslynReflection.Parsers.SourceCode.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Test.TestHelpers.Extensions
{
    internal static class RawScannedNamespaceExtensions
    {
        internal static RawScannedType AddType(this RawScannedNamespace scannedNamespace, string name)
        {
            return new(name, scannedNamespace, ClassDeclaration(name), null);
        }
    }
}