using System;
using System.Linq;
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
            type.ParentType = GetType(type, fullname);
            return type;
        }


        public static T ImplementInterface<T>(this T type, string interfaceName)
        where T: ScannedType
        {
            var found = GetType(type, interfaceName);
            if (found is not ScannedInterface scannedInterface)
            {
                throw new ArgumentException($"type '{interfaceName}' is not an interface");
            }
            
            type.ImplementedInterfaces.Add(scannedInterface);
            return type;
        }
        
        
        private static ScannedType GetType<T>(T type, string fullname) where T : ICanNavigateToModule
        {
            var parentType = type.Module.GetAllAvailableTypes().SingleOrDefault(t => t.FullyQualifiedName() == fullname);

            if (parentType == null)
            {
                throw new ArgumentException($"Cannot find type '{fullname}' in this module", nameof(fullname));
            }

            return parentType;
        }
    }
}