using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class TypeDeclarationParser
    {
        private readonly List<IScannedUsing> _scannedUsings;
        private Lazy<ClassDeclarationParser> _classDeclarationParser;
        private Lazy<InterfaceDeclarationParser> _interfaceDeclarationParser;
        private Lazy<RecordDeclarationParser> _recordDeclarationParser;

        internal TypeDeclarationParser(TypeListList typeListList, List<IScannedUsing> scannedUsings,
            ScannedType? surroundingType = null)
        {
            _scannedUsings = scannedUsings;
            _classDeclarationParser = new(() => new ClassDeclarationParser(typeListList, scannedUsings, surroundingType));
            _interfaceDeclarationParser = new (() => new InterfaceDeclarationParser(typeListList, scannedUsings, surroundingType));
            _recordDeclarationParser =
                new(() => new RecordDeclarationParser(typeListList, scannedUsings, surroundingType));
        }

        internal void ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            ScannedType type = typeDeclaration switch
            {
                ClassDeclarationSyntax classDecl => _classDeclarationParser.Value.ParseDeclaration(classDecl),
                InterfaceDeclarationSyntax interfaceDecl => _interfaceDeclarationParser.Value.ParseDeclaration(interfaceDecl),
                RecordDeclarationSyntax recordDecl => _recordDeclarationParser.Value.ParseDeclaration(recordDecl),
                StructDeclarationSyntax structDeclarationSyntax => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeDeclaration), typeDeclaration.GetType(),
                    "Unknown TypeDeclaration type")
            };

            type.Usings.AddRange(_scannedUsings);
        }

        
    }
}