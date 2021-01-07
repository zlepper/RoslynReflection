using System;

namespace RoslynReflection.Models.Assembly
{
    public interface IScannedAssemblyType : IScannedType
    {
        public Type Type { get; }

    }
}