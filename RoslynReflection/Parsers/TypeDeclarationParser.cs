using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers
{
    internal class TypeDeclarationParser
    {
        private Lazy<ClassDeclarationParser> _classDeclarationParser;
        private SourceType? _surroundingType;
        
        internal TypeDeclarationParser(SourceClassList classList, SourceType? surroundingType = null)
        {
            _surroundingType = surroundingType;
            _classDeclarationParser = new(() => new ClassDeclarationParser(classList, surroundingType));
        }

        internal SourceType ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            return (typeDeclaration) switch
            {
                ClassDeclarationSyntax classDecl => _classDeclarationParser.Value.ParseClassDeclaration(classDecl),
                InterfaceDeclarationSyntax interfaceDeclarationSyntax => throw new NotImplementedException(),
                RecordDeclarationSyntax recordDeclarationSyntax => throw new NotImplementedException(),
                StructDeclarationSyntax structDeclarationSyntax => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeDeclaration), typeDeclaration.GetType(),
                    "Unknown TypeDeclaration type")
            };
        }

        
    }
}