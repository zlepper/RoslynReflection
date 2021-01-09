using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Models.Markers;

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

            if (type is ICanBePartial canBePartial)
            {
                CheckPartial(canBePartial, typeDeclaration);
            }

            if (type is ICanBeAbstract canBeAbstract)
            {
                CheckAbstract(canBeAbstract, typeDeclaration);
            }

            CheckBaseTypes(type, typeDeclaration);

            type.Usings.AddRange(_scannedUsings);
        }

        private void CheckPartial(ICanBePartial canBePartial, TypeDeclarationSyntax typeDeclaration)
        {
            canBePartial.IsPartial = typeDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.PartialKeyword);
        }

        private void CheckAbstract(ICanBeAbstract canBeAbstract, TypeDeclarationSyntax typeDeclaration)
        {
            canBeAbstract.IsAbstract = typeDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.AbstractKeyword);
        }

        private void CheckBaseTypes(ScannedType scannedType, TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration.BaseList == null)
            {
                return;
            }

            foreach (var baseTypeSyntax in typeDeclaration.BaseList.Types)
            {
                if (baseTypeSyntax.Type is IdentifierNameSyntax identifier)
                {
                    var name = identifier.Identifier.ValueText;
                    scannedType.BaseTypes.Add(new UnlinkedType(name.Trim()));
                }
                else
                {
                    throw new NotImplementedException("Unknown baseTypeSyntax.Type type. Please report a bug.");
                }

            }
        }
        
    }
}