using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class TypeDeclarationParser
    {
        private Lazy<ClassDeclarationParser> _classDeclarationParser;

        internal TypeDeclarationParser(ClassList classList, ScannedType? surroundingType = null)
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