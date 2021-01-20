using System;
using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public static class ScannedTypeExtensions
    {
        public static ScannedType AddNestedClass(this ScannedType type, string name)
        {
            return new(name, type.Namespace)
            {
                BaseType = type
            };
        }

        public static ScannedType AddAttribute(this ScannedType type, Attribute attribute)
        {
            type.Attributes.Add(attribute);
            return type;
        }

        public static ScannedType MakePartial(this ScannedType type)
        {
            type.IsPartial = true;
            return type;
        }
    }
}