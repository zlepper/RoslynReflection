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

        internal TypeDeclarationParser(ClassList classList, List<IScannedUsing> scannedUsings,
            ScannedType? surroundingType = null)
        {
            _scannedUsings = scannedUsings;
            _classDeclarationParser = new(() => new ClassDeclarationParser(classList, scannedUsings, surroundingType));
        }

        internal void ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            ScannedType type = typeDeclaration switch
            {
                ClassDeclarationSyntax classDecl => _classDeclarationParser.Value.ParseClassDeclaration(classDecl),
                InterfaceDeclarationSyntax interfaceDeclarationSyntax => throw new NotImplementedException(),
                RecordDeclarationSyntax recordDeclarationSyntax => throw new NotImplementedException(),
                StructDeclarationSyntax structDeclarationSyntax => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeDeclaration), typeDeclaration.GetType(),
                    "Unknown TypeDeclaration type")
            };

            type.Usings.AddRange(_scannedUsings);
        }

        
    }
}