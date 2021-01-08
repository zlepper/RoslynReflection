using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Collections
{
    public class InterfaceList : TypeList<ScannedSourceInterface, InterfaceDeclarationSyntax>
    {
        public InterfaceList(ScannedNamespace ns) : base(ns)
        {
        }

        protected override ScannedSourceInterface InitType(string name, InterfaceDeclarationSyntax declarationSyntax,
            ScannedType? surroundingType = null)
        {
            return new(declarationSyntax, Namespace, name, surroundingType);
        }
    }
}