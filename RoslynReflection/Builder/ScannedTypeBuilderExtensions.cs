using System;
using System.Linq;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Builder
{
    public static class ScannedTypeBuilderExtensions
    {
        public static T MakeAbstract<T>(this T type)
            where T : ICanBeAbstract
        {
            type.IsAbstract = true;
            return type;
        }

        public static T MakePartial<T>(this T type)
            where T : ICanBePartial
        {
            type.IsPartial = true;
            return type;
        }

        public static T InheritFrom<T>(this T type, string fullname)
            where T : ICanInherit
        {
            type.ParentType = new TypeReference(GetType(type, fullname));
            return type;
        }

        public static T InheritFromGenericType<T>(this T type, string fullname, params string[] genericArguments)
            where T : ICanInherit
        {
            var parentType = GetType(type, fullname);

            
            if (parentType.GenericTypeArguments.Count != genericArguments.Length)
            {
                throw new ArgumentException(
                    $"Number of generic arguments doesn't match expected. Excepted {parentType.GenericTypeArguments.Count}, got {genericArguments.Length}.",
                    nameof(genericArguments));
            }
            
            var arguments = genericArguments.Select(t => GetTypeReference(type, t)).ToValueList();
            type.ParentType = new GenericTypeReference(parentType, arguments);
            return type;
        }

        private static ITypeReference GetTypeReference<T>(T type, string name)
        where T : ICanInherit
        {
            var genericTypeArgument = type.GenericTypeArguments.FirstOrDefault(a => a.Name == name);

            if (genericTypeArgument != null)
            {
                return new GenericArgumentReference(genericTypeArgument.Name);
            }

            var actualType = GetType(type, name);

            return new TypeReference(actualType);
        }

        public static T ImplementInterface<T>(this T type, string interfaceName)
            where T : ScannedType
        {
            var found = GetType(type, interfaceName);
            if (found is not ScannedInterface scannedInterface)
            {
                throw new ArgumentException($"type '{interfaceName}' is not an interface");
            }

            type.ImplementedInterfaces.Add(scannedInterface);
            return type;
        }


        private static ScannedType GetType<T>(T type, string fullname) where T : IScannedType
        {
            var at = new AvailableTypes(type);

            if (at.TryGetType(type, fullname, out var parentType))
            {
                return parentType!;
            }

            throw new ArgumentException($"Cannot find type '{fullname}' in this module", nameof(fullname));
        }

        public static T WithGenericTypeArgument<T>(this T type, string name)
            where T : ScannedType
        {
            type.AddGenericTypeArgument(name);
            return type;
        }

        public static GenericTypeParameter AddGenericTypeArgument<T>(this T type, string name)
            where T : ScannedType
        {
            return new GenericTypeParameter(type, name);
        }
    }
}