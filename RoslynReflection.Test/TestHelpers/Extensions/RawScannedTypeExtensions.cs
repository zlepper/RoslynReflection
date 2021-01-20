using RoslynReflection.Parsers.SourceCode.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Test.TestHelpers.Extensions
{
    internal static class RawScannedTypeExtensions
    {
        internal static RawScannedType AddNestedType(this RawScannedType type, string name)
        {
            return new(name, type.Namespace, ClassDeclaration(name), type);
        }

        internal static RawScannedType AddUsing(this RawScannedType type, string ns)
        {
            type.Usings.Add(new ScannedUsing(ns));
            return type;
        }

        internal static RawScannedType AddUsingAlias(this RawScannedType type, string ns, string alias)
        {
            type.Usings.Add(new ScannedUsingAlias(ns, alias));
            return type;
        }
    }
}