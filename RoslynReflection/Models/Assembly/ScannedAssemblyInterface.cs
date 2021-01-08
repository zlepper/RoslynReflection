using System;
using System.Text;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models.Assembly
{
    public record ScannedAssemblyInterface : ScannedInterface, IScannedAssemblyType
    {
        public ScannedAssemblyInterface(Type type, ScannedNamespace ns, ScannedType? surroundingType = null) : base(ns, type.Name, surroundingType)
        {
            Type = type;
        }

        public Type Type { get; }
        
        
        protected override bool PrintMembers(StringBuilder builder)
        {
            InternalPrintMembers(builder.StartAppendingFields())
                .AppendField(nameof(Type), Type);

            return true;
        }
    }
}