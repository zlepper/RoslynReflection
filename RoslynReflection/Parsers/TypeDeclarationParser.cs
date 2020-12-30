using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers
{
    internal class TypeDeclarationParser
    {
        private Lazy<ClassDeclarationParser> _classDeclarationParser;

        internal TypeDeclarationParser(SourceClassList classList, SourceType? surroundingType = null)
        {
            _classDeclarationParser = new(() => new ClassDeclarationParser(classList, surroundingType));
        }

        internal void ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            switch (typeDeclaration)
            {
                case ClassDeclarationSyntax classDecl:
                    _classDeclarationParser.Value.ParseClassDeclaration(classDecl);
                    break;
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                case RecordDeclarationSyntax recordDeclarationSyntax:
                case StructDeclarationSyntax structDeclarationSyntax:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeDeclaration), typeDeclaration.GetType(),
                        "Unknown TypeDeclaration type");
            }
        }

        
    }
}