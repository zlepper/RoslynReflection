using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class InterfaceDeclarationParser : BaseConcreteTypeDeclarationParser<ScannedSourceInterface, InterfaceDeclarationSyntax>
    {
        public InterfaceDeclarationParser(TypeListList typeListList, List<IScannedUsing> scannedUsings, ScannedType? surroundingType) : base(typeListList, scannedUsings, surroundingType)
        {
        }

        protected override TypeList<ScannedSourceInterface, InterfaceDeclarationSyntax> TypeList =>
            TypeListList.InterfaceList;
    }
}