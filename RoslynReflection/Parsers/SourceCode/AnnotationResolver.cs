using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class AnnotationResolver
    {
        private readonly AvailableTypes _availableTypes;

        public AnnotationResolver(AvailableTypes availableTypes)
        {
            _availableTypes = availableTypes;
        }

        public void ResolveAnnotations(IEnumerable<RawScannedType> sourceTypes)
        {
            foreach (var type in sourceTypes) ResolveAnnotations(type);
        }

        private void ResolveAnnotations(RawScannedType sourceType)
        {
            var attributes = sourceType.TypeDeclarationSyntax.SelectMany(s => s.AttributeLists)
                .SelectMany(l => l.Attributes);
            foreach (var attribute in attributes)
                throw new NotImplementedException("TODO");
            // ResolveAnnotation(attribute, sourceType);
        }

        private void ResolveAnnotation(AttributeSyntax attribute, ScannedType sourceType)
        {
            var attributeName = attribute.Name.GetText().ToString();

            var alternativeName = attributeName + "Attribute";

            // if (_availableTypes.TryGetType(sourceType, attributeName, out var attributeType) ||
                // _availableTypes.TryGetType(sourceType, alternativeName, out attributeType))
                throw new NotImplementedException("TODO:");
            // if (attributeType is IScannedAssemblyType assemblyType)
            // {
            //     var attributeInstance = InitializeAttribute(assemblyType.Type, attribute);
            //     if (attributeInstance != null)
            //     {
            //         sourceType.Attributes.Add(attributeInstance);
            //     }
            // }
        }

        private static object? InitializeAttribute(Type type, AttributeSyntax attributeSyntax)
        {
            if (attributeSyntax.ArgumentList == null || attributeSyntax.ArgumentList.Arguments.Count == 0)
            {
                var constructor = type.GetConstructor(Array.Empty<Type>());
                if (constructor != null) return constructor.Invoke(Array.Empty<object>());

                return null;
            }

            var arguments = attributeSyntax.ArgumentList.Arguments;


            return null;
        }
    }
}