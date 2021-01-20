using System.Collections.Generic;
using JetBrains.Annotations;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Parsers
{
    internal class TypeReferenceResolver
    {
        private AvailableTypes _availableTypes;

        public TypeReferenceResolver(AvailableTypes availableTypes)
        {
            _availableTypes = availableTypes;
        }

        public void ResolveUnlinkedTypes(IEnumerable<ScannedType> types)
        {
            foreach (var type in types)
            {
                LinkBaseType(type);
            }
        }

        private void LinkBaseType(ScannedType type)
        {
            foreach (var baseType in type.BaseTypes)
            {
                if (!GetActualBaseType(type, baseType, out var actualType)) continue;

                if (actualType is ScannedInterface scannedInterface)
                {
                    type.ImplementedInterfaces.Add(scannedInterface);
                }
                else
                {
                    // We are missing a couple of cases around structs inheriting from class, but the 
                    // c# compiler will handle those.
                    if (type is ICanInherit canInherit)
                    {
                        canInherit.ParentType = new TypeReference(actualType!);
                    }
                }
            }
            
            type.BaseTypes.Clear();
        }

        [ContractAnnotation("=> true, actualType: notnull; => false, actualType: null")]
        private bool GetActualBaseType(ScannedType type, ScannedType baseType, out ScannedType? actualType)
        {
            if (baseType is UnlinkedType unlinkedType)
            {
                if (!_availableTypes.TryGetType(type, unlinkedType.Name, out var matchedType))
                {
                    actualType = null;
                    return false;
                }

                actualType = matchedType!;
            }
            else
            {
                actualType = baseType;
            }

            return true;
        }
    }
}