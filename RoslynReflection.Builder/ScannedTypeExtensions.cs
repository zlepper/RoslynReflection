using System;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser;

namespace RoslynReflection.Builder
{
    public static class ScannedTypeExtensions
    {
        private static ScannedType AddNestedType(ScannedType parent, string name, Action<ScannedType> modify)
        {
            Guard.AgainstNull(parent, nameof(parent));
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(modify, nameof(modify));

            var type = new ScannedType(name, parent.Namespace, parent);
            parent.NestedTypes.Add(type);
            parent.Namespace.AddType(type);
            
            modify(type);
            
            return type;
        }
        
        public static ScannedType AddNestedClass(this ScannedType type, string name)
        {
            return AddNestedType(type, name, t => t.IsClass = true);
        }

        public static ScannedType AddNestedInterface(this ScannedType type, string name)
        {
            return AddNestedType(type, name, t => t.IsInterface = true);
        }

        public static ScannedType AddNestedRecord(this ScannedType type, string name)
        {
            return AddNestedType(type, name, t => t.IsClass = t.IsRecord = true);
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

        public static ScannedType MakeAbstract(this ScannedType type)
        {
            Guard.AgainstNull(type, nameof(type));

            type.IsAbstract = true;
            return type;
        }
    }
}