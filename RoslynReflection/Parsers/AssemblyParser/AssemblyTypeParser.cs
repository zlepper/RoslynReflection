using System;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyTypeParser
    {
        private readonly ScannedNamespace _scannedNamespace;

        public AssemblyTypeParser(ScannedNamespace scannedNamespace)
        {
            _scannedNamespace = scannedNamespace;
        }

        internal ScannedType ParseType(Type type)
        {
            var scannedType = new ScannedType(type.Name, _scannedNamespace)
            {
                ClrType = type
            };

            return scannedType;
        }
    }
}