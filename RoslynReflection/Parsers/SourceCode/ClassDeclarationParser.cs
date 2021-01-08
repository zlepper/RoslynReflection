using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class
        ClassDeclarationParser : BaseConcreteTypeDeclarationParser<ScannedSourceClass, ClassDeclarationSyntax>
    {
        public ClassDeclarationParser(TypeListList typeListList, List<IScannedUsing> scannedUsings,
            ScannedType? surroundingType) : base(typeListList, scannedUsings, surroundingType)
        {
        }

        protected override TypeList<ScannedSourceClass, ClassDeclarationSyntax> TypeList => TypeListList.ClassList;
    }
}