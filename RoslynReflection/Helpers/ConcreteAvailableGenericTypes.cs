using System.Collections.Generic;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Helpers
{
    internal class ConcreteAvailableGenericTypes
    {
        private readonly Dictionary<TypePair, ScannedType> _existingGenericTypes = new();

        internal ScannedType FindOrCreateGenericType(ScannedType originalType, ValueList<ScannedType> typeArguments)
        {
            var pair = new TypePair(originalType, typeArguments);

            if (_existingGenericTypes.TryGetValue(pair, out var genericType))
            {
                return genericType;
            }

            genericType = originalType with
            {
                GenericTypeArguments = typeArguments,
                GenericTypeParameters = new(),
                ContainsGenericParameters = false,
                IsConstructedGenericType = true,
                IsGenericTypeDefinition = false
            };

            _existingGenericTypes[pair] = genericType;

            return genericType;
        }

        private struct TypePair
        {
            private readonly ScannedType _mainType;

            private readonly ValueList<ScannedType> _typeArguments;

            public TypePair(ScannedType mainType, ValueList<ScannedType> typeArguments)
            {
                _mainType = mainType;
                _typeArguments = typeArguments;
            }

            public bool Equals(TypePair other)
            {
                return _mainType.Equals(other._mainType) && _typeArguments.Equals(other._typeArguments);
            }

            public override bool Equals(object? obj)
            {
                return obj is TypePair other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_mainType.GetHashCode() * 397) ^ _typeArguments.GetHashCode();
                }
            }

            public static bool operator ==(TypePair left, TypePair right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(TypePair left, TypePair right)
            {
                return !left.Equals(right);
            }
        }
    }
}