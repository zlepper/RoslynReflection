using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Parsers.SourceCode.Collections;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class TypeDeclarationParser
    {
        private readonly List<IScannedUsing> _scannedUsings;
        private readonly RawScannedType? _surroundingType;
        private readonly TypeList _typeList;

        internal TypeDeclarationParser(TypeList typeList, List<IScannedUsing> scannedUsings,
            RawScannedType? surroundingType = null)
        {
            _typeList = typeList;
            _scannedUsings = scannedUsings;
            _surroundingType = surroundingType;
        }

        internal void ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            var type = _typeList.GetType(typeDeclaration, _surroundingType);
            
            type.Usings.AddRange(_scannedUsings);

            var nestedParser = new TypeDeclarationParser(_typeList, _scannedUsings, type);
            foreach (var nestedType in typeDeclaration.Members.OfType<TypeDeclarationSyntax>())
            {
                nestedParser.ParseTypeDeclaration(nestedType);
            }
        }
    }
}