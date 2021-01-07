using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynReflection.Models.Source
{
    public interface IScannedSourceType : IScannedType
    {
        TypeDeclarationSyntax DeclarationSyntax { get; }
    }
}