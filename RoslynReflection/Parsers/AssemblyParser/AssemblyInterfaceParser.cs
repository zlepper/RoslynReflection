using System;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyInterfaceParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;

        public AssemblyInterfaceParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType = null)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
        }

        public ScannedAssemblyInterface ParseInterface(Type type)
        {
            return new (type, _scannedNamespace, _surroundingType);
        }
    }
}