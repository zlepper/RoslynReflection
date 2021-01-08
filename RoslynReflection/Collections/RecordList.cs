using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Collections
{
    internal class RecordList : TypeList<ScannedSourceRecord, RecordDeclarationSyntax>
    {
        public RecordList(ScannedNamespace ns) : base(ns)
        {
        }

        protected override ScannedSourceRecord InitType(string name, RecordDeclarationSyntax declarationSyntax,
            ScannedType? surroundingType = null)
        {
            return new(declarationSyntax, Namespace, name, surroundingType);
        }
    }
}