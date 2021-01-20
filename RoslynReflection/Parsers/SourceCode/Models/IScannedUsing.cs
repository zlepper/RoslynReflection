using JetBrains.Annotations;
using RoslynReflection.Helpers;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal interface IScannedUsing
    {
        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        internal bool TryGetType(string typeName, AvailableTypes availableTypes,
            out ScannedType? type);
    }
}