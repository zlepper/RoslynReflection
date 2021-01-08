using System;
using System.Text;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models.Assembly
{
    public record ScannedAssemblyInterface : ScannedInterface, IScannedAssemblyType
    {
        public ScannedAssemblyInterface(Type type, ScannedNamespace ns, string name,
            ScannedType? surroundingType = null) : base(ns, name, surroundingType)
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