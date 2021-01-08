using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class RecordDeclarationParser : BaseConcreteTypeDeclarationParser<ScannedSourceRecord, RecordDeclarationSyntax>
    {
        public RecordDeclarationParser(TypeListList typeListList, List<IScannedUsing> scannedUsings, ScannedType? surroundingType) : base(typeListList, scannedUsings, surroundingType)
        {
        }

        protected override TypeList<ScannedSourceRecord, RecordDeclarationSyntax> TypeList => TypeListList.RecordList;
        
        
        protected override void ModifySpecifics(RecordDeclarationSyntax typeDeclaration, ScannedSourceRecord type)
        {
            type.IsAbstract = typeDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.AbstractKeyword);
        }
    }
}