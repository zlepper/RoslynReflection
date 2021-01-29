using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.Linkers
{
    internal class TypeReferenceResolver
    {
        private AvailableTypes _availableTypes;

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
                    switch (baseTypeSyntax)
                    {
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                            switch (simpleBaseTypeSyntax.Type)
                            {
                                case IdentifierNameSyntax nameSyntax:
                                {
                                    var baseTypeName = nameSyntax.Identifier.Text.Trim();
                                    if (_availableTypes.TryGetType(raw, baseTypeName, out var baseType))
                                    {
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
                                            throw new Exception("Got base type that is neither a class nor an interface");
                                        }
                                    }

                                    break;
                                }
                                case GenericNameSyntax genericNameSyntax:
                                {
                                    var baseTypeName = genericNameSyntax.Identifier.Text.Trim();
                                    if (_availableTypes.TryGetType(raw, baseTypeName, out var baseType))
                                    {
                                        
                                    }


                                    break;
                                }
                                default:
                                    throw new NotImplementedException(
                                        $"Missing implementation for SimpleBaseTypeSyntax.Type not being IdentiferNameSyntax. Got: {simpleBaseTypeSyntax.Type.GetType()}");
                            }

                            break;
                        default:
                            throw new NotImplementedException(
                                $"Missing implementation for base syntax: {baseTypeSyntax.GetType()}");
                    }
                }
            }
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
            
            if (_availableTypes.TryGetType(reflectedBaseType.Namespace ?? "", reflectedBaseType.GetNonGenericTypeName(), out var baseType))
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

                if (_availableTypes.TryGetType(interfaceType.Namespace ?? "", interfaceType.GetNonGenericTypeName(), out var scannedInterfaceType))
                {
                    type.ImplementedInterfaces.Add(scannedInterfaceType);
                }
            }
        }
    }
}