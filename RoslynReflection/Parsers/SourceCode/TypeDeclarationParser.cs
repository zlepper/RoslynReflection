using System;
using Microsoft.CodeAnalysis.CSharp;
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
            ScannedType type = typeDeclaration switch
            {
                ClassDeclarationSyntax classDecl => _classDeclarationParser.Value.ParseClassDeclaration(classDecl),
                InterfaceDeclarationSyntax interfaceDeclarationSyntax => throw new NotImplementedException(),
                RecordDeclarationSyntax recordDeclarationSyntax => throw new NotImplementedException(),
                StructDeclarationSyntax structDeclarationSyntax => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeDeclaration), typeDeclaration.GetType(),
                    "Unknown TypeDeclaration type")
            };

            foreach (var attributeList in typeDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    Console.WriteLine(attribute);
                }
            }

        }

        
    }
}