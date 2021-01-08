using System;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Parsers.AssemblyParser
{
    public class AssemblyRecordParser
    {
        
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;

        public AssemblyRecordParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType = null)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
        }

        public ScannedAssemblyRecord ParseRecord(Type type)
        {
            return new(type, _scannedNamespace, _surroundingType);
        }
    }
}