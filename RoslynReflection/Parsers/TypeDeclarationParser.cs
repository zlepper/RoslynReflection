using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers
{
    internal class TypeDeclarationParser
    {
        private Lazy<ClassDeclarationParser> _classDeclarationParser;
        
        internal TypeDeclarationParser(SourceClassList classList)
        {
            _classDeclarationParser = new(() => new ClassDeclarationParser(classList));
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