using System;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyClassParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;

        public AssemblyClassParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType = null)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
        }

        public ScannedAssemblyClass ParseClass(Type type)
        {
            return new(type, _scannedNamespace, _surroundingType);
        }
    }
}