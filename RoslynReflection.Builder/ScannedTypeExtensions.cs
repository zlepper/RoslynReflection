using System;
using RoslynReflection.Helpers;
using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public static class ScannedTypeExtensions
    {
        public static ScannedType AddNestedClass(this ScannedType type, string name)
        {
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(name, nameof(name));
            
            return new(name, type.Namespace)
            {
                SurroundingType = type,
                IsClass = true
            };
        }

        public static ScannedType AddAttribute(this ScannedType type, Attribute attribute)
        {
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(attribute, nameof(attribute));
            
            type.Attributes.Add(attribute);
            return type;
        }

        public static ScannedType MakePartial(this ScannedType type)
        {
            Guard.AgainstNull(type, nameof(type));
            
            type.IsPartial = true;
            return type;
        }
    }
}