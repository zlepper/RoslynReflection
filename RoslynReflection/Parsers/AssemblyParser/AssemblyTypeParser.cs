using System;
using System.Linq;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyTypeParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;

        public AssemblyTypeParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType = null)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
        }

        internal ScannedType ParseType(Type type)
        {
            var scannedType = GetConcreteType(type);

            foreach (var attribute in type.GetCustomAttributes(false))
            {
                if (RoslynReflectionConstants.HiddenNamespaces.Contains(attribute.GetType().Namespace)) continue;
                scannedType.Attributes.Add(attribute);
            }

            return scannedType;
        }

        private ScannedType GetConcreteType(Type type)
        {
            if (type.IsClass)
            {
                if (type.IsRecord())
                {
                    var parser = new AssemblyRecordParser(_scannedNamespace, _surroundingType);
                    return parser.ParseRecord(type);
                }
                else
                {
                    var parser = new AssemblyClassParser(_scannedNamespace, _surroundingType);
                    return parser.ParseClass(type);
                }
            }

            if (type.IsInterface)
            {
                var parser = new AssemblyInterfaceParser(_scannedNamespace, _surroundingType);
                return parser.ParseInterface(type);
            }

            // TODO: Bad fallback for now, until we have support for all sorts of combinations
            return new AssemblyClassParser(_scannedNamespace, _surroundingType).ParseClass(type);
        }
    }

    internal static class TypeExtensions
    {
        internal static bool IsRecord(this Type type)
        {
            // "Borrowed" from https://stackoverflow.com/a/64810188/3950006
            return type.GetMethods().Any(m => m.Name == "<Clone>$");
        }
    }
}