﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers
{
    internal class RawScannedTypeLinker
    {
        internal static ScannedModule LinkRawTypes(RawScannedModule module)
        {
            var fullModule = GetUnlinkedScannedModule(module);

            fullModule.DependsOn.AddRange(module.DependsOn);

            foreach (var scannedType in fullModule.Types())
            {
                SetTypeDetails(scannedType);
            }

            return fullModule;
        }

        internal static void SetTypeDetails(ScannedType type)
        {
            var raw = type.RawScannedType ?? throw new Exception(
                $"Got scanned type that was supposed to be from raw code, but didn't have a RawScannedType attached. Type: '{type.Name}'");

            var declarations = raw.TypeDeclarationSyntax;

            type.IsPartial = declarations.Count > 1 ||
                             declarations[0].Modifiers.Any(m => m.Kind() == SyntaxKind.PartialKeyword);

            foreach (var declaration in declarations)
            {
                type.IsRecord = declaration is RecordDeclarationSyntax;
                type.IsClass = type.IsRecord || declaration is ClassDeclarationSyntax;
                type.IsInterface = declaration is InterfaceDeclarationSyntax;
                type.IsAbstract = type.IsAbstract ||
                                  declaration.Modifiers.Any(m => m.Kind() == SyntaxKind.AbstractKeyword);
            }
        }


        internal static ScannedModule GetUnlinkedScannedModule(RawScannedModule rawModule)
        {
            var scannedModule = new ScannedModule();

            foreach (var rawNamespace in rawModule.Namespaces)
            {
                var scannedNamespace = new ScannedNamespace(scannedModule, rawNamespace.Name);

                foreach (var rawType in rawNamespace.Types.Where(t => t.SurroundingType == null))
                {
                    CreateScannedType(scannedNamespace, rawType, null);
                }
            }

            return scannedModule;
        }

        private static ScannedType CreateScannedType(ScannedNamespace ns, RawScannedType rawType,
            ScannedType? surroundingType)
        {
            var type = new ScannedType(rawType.Name, ns, surroundingType)
            {
                RawScannedType = rawType
            };

            foreach (var rawNestedType in rawType.NestedTypes)
            {
                CreateScannedType(ns, rawNestedType, type);
            }

            return type;
        }
    }

    public abstract partial class MyClass
    {
    }

    public partial class MyClass
    {
    }
}