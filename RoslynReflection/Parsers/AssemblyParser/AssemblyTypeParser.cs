using System;
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
                scannedType.Attributes.Add(attribute);
            }

            return scannedType;
        }

        private ScannedType GetConcreteType(Type type)
        {
            if (type.IsClass)
            {
                var parser = new AssemblyClassParser(_scannedNamespace, _surroundingType);
                return parser.ParseClass(type);
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
}