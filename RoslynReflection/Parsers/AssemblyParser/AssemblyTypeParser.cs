using System;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyTypeParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;

        public AssemblyTypeParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
        }

        internal ScannedType ParseType(Type type)
        {
            var scannedType = new ScannedType(type.Name, _scannedNamespace, _surroundingType)
            {
                ClrType = type
            };

            return scannedType;
        }
    }
}