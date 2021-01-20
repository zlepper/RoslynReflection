﻿using System;
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
            
            type.Usings.AddRange(_scannedUsings);
            
            CheckPartial(type, typeDeclaration);

            CheckAbstract(type, typeDeclaration);

            CheckBaseTypes(type, typeDeclaration);
            
            CheckTypeParameters(type, typeDeclaration);
            
            // typeDeclaration.type
        }

        private void CheckPartial(ScannedType type, TypeDeclarationSyntax typeDeclaration)
        {
            if (type is ICanBePartial canBePartial)
            {
                canBePartial.IsPartial = typeDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.PartialKeyword);
            }
        }

        private void CheckAbstract(ScannedType type, TypeDeclarationSyntax typeDeclaration)
        {

            if (type is ICanBeAbstract canBeAbstract)
            {
                canBeAbstract.IsAbstract = typeDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.AbstractKeyword);
            }
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
                    if (baseTypeSyntax.Type is GenericNameSyntax nameSyntax)
                    {
                        if (nameSyntax.IsUnboundGenericName)
                        {
                            throw new NotImplementedException(
                                "Support for unbound generic types is currently not implemented");
                        }

                        var name = nameSyntax.Identifier.ValueText;
                        
                        foreach (var typeArgumentSyntax in nameSyntax.TypeArgumentList.Arguments)
                        {
                            Console.WriteLine(typeArgumentSyntax);
                        }
                        
                        Console.WriteLine(nameSyntax);
                    }
                    else
                    {
                        throw new NotImplementedException("Unknown baseTypeSyntax.Type type. Please report a bug.");
                    }
                }

            }
        }

        private void CheckTypeParameters(ScannedType type, TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration.TypeParameterList == null)
            {
                return;
            }

            foreach (var typeParameterSyntax in typeDeclaration.TypeParameterList.Parameters)
            {
                var name = typeParameterSyntax.Identifier.Text.Trim();
                var _ = new GenericTypeParameter(type, name);
            }
        }
    }
}