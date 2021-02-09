using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Exceptions;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.Linkers
{
    internal class TypeReferenceResolver
    {
        private AvailableTypes _availableTypes;
        private ConcreteAvailableGenericTypes _concreteAvailableGenericTypes = new();

        internal TypeReferenceResolver(AvailableTypes availableTypes)
        {
            _availableTypes = availableTypes;
        }

        internal void ResolveUnlinkedTypes(IEnumerable<ScannedType> types)
        {
            foreach (var scannedType in types)
            {
                LinkBaseType(scannedType);
            }
        }

        private void LinkBaseType(ScannedType type)
        {
            switch (type)
            {
                case {RawScannedType: { } raw}:
                    LinkBaseTypeFromRaw(type, raw);
                    break;
                case {ClrType: { } t}:
                    LinkBaseTypeFromReflection(type, t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private void LinkBaseTypeFromRaw(ScannedType type, RawScannedType raw)
        {
            foreach (var typeDeclaration in raw.TypeDeclarationSyntax)
            {
                if (typeDeclaration.BaseList == null)
                {
                    continue;
                }

                foreach (var baseTypeSyntax in typeDeclaration.BaseList.Types)
                {
                    var baseType = ParseBaseTypeSyntax(type, baseTypeSyntax);

                    if (baseType.IsClass)
                    {
                        type.BaseType = baseType;
                    } 
                    else if (baseType.IsInterface)
                    {
                        type.ImplementedInterfaces.Add(baseType);
                    }
                    else
                    {
                        throw new ProbablyABugException("Got base type that is neither a class nor an interface");
                    }
                }
            }
        }

        private ScannedType ParseBaseTypeSyntax(ScannedType declaringType, BaseTypeSyntax baseTypeSyntax)
        {
            return baseTypeSyntax switch
            {
                SimpleBaseTypeSyntax simpleBaseTypeSyntax => ParseTypeSyntax(declaringType, simpleBaseTypeSyntax.Type),
                _ => throw new NotImplementedException(
                    $"Missing implementation for base syntax: {baseTypeSyntax.GetType()}")
            };
        }

        private ScannedType ParseTypeSyntax(ScannedType declaringType, TypeSyntax typeSyntax)
        {
            var raw = declaringType.RawScannedType ??
                      throw new ProbablyABugException(
                          "Attempted to parse type syntax without an attached RawScannedType");

            return typeSyntax switch
            {
                IdentifierNameSyntax nameSyntax => ParseIdentifierNameSyntax(declaringType, nameSyntax, raw),
                GenericNameSyntax genericNameSyntax => ParseGenericNameSyntax(declaringType, genericNameSyntax, raw),
                _ => throw new NotImplementedException(
                    $"Missing implementation for unknown SimpleBaseTypeSyntax.Type. Got: {typeSyntax.GetType()}")
            };
        }

        private ScannedType ParseIdentifierNameSyntax(ScannedType declaringType, IdentifierNameSyntax nameSyntax,
            RawScannedType raw)
        {
            var baseTypeName = nameSyntax.Identifier.Text.Trim();

            var matchedGenericType =
                declaringType.GenericTypeParameters.FirstOrDefault(p => p.Name == baseTypeName);

            if (matchedGenericType != null)
            {
                return matchedGenericType;
            }

            if (_availableTypes.TryGetType(raw, baseTypeName, out var baseType))
            {
                return baseType;
            }

            return CreateUnknownType(baseTypeName, declaringType);
        }

        private ScannedType ParseGenericNameSyntax(ScannedType declaringType, GenericNameSyntax genericNameSyntax,
            RawScannedType raw)
        {
            var baseTypeName = genericNameSyntax.Identifier.Text.Trim();
            if (!_availableTypes.TryGetType(raw, baseTypeName, out var originalBaseType))
            {
                return CreateUnknownType(baseTypeName, declaringType);
            }

            var typeArguments = genericNameSyntax.TypeArgumentList.Arguments
                .Select((typeArgument) => ParseTypeSyntax(declaringType, typeArgument))
                .ToValueList();

            if (!typeArguments.Any(ta => ta.IsGenericType && ta.ContainsGenericParameters))
            {
                return _concreteAvailableGenericTypes.FindOrCreateGenericType(originalBaseType, typeArguments);
            }

            var concreteGenericArguments = typeArguments
                .Where(ta => !ta.IsGenericType || !ta.ContainsGenericParameters)
                .ToValueList();

            var unresolvedGenericParameters = typeArguments
                .Where(ta => ta.IsGenericType && ta.ContainsGenericParameters)
                .ToValueList();

            return originalBaseType with
            {
                GenericTypeArguments = concreteGenericArguments,
                GenericTypeParameters = unresolvedGenericParameters,
                UnderlyingType = originalBaseType,
            };
        }

        private ScannedType CreateUnknownType(string name, ScannedType declaringType)
        {
            return new(name, declaringType.Namespace, declaringType);
        }

        private void LinkBaseTypeFromReflection(ScannedType type, Type reflectedType)
        {
            if (reflectedType.IsGenericType)
            {
                // TODO
                return;
            }

            if (reflectedType.BaseType == null || reflectedType.BaseType.IsGenericType)
            {
                return;
            }

            var reflectedBaseType = reflectedType.BaseType;

            if (_availableTypes.TryGetType(reflectedBaseType.Namespace ?? "", reflectedBaseType.GetNonGenericTypeName(),
                out var baseType))
            {
                type.BaseType = baseType;
            }

            foreach (var interfaceType in reflectedType.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    // TODO
                    continue;
                }

                if (_availableTypes.TryGetType(interfaceType.Namespace ?? "", interfaceType.GetNonGenericTypeName(),
                    out var scannedInterfaceType))
                {
                    type.ImplementedInterfaces.Add(scannedInterfaceType);
                }
            }
        }
    }
}