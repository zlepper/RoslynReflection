using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Collections
{
    internal class ClassList : TypeList<ScannedSourceClass, ClassDeclarationSyntax>
    {
        public ClassList(ScannedNamespace ns) : base(ns)
        {
        }

        protected override ScannedSourceClass InitType(string name, ClassDeclarationSyntax declarationSyntax, ScannedType? surroundingType = null)
        {
            return new(declarationSyntax, Namespace, name, surroundingType);
        }
    }
}