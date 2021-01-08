using System;
using System.Text;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models.Assembly
{
    public record ScannedAssemblyRecord : ScannedRecord, IScannedAssemblyType
    {
        public Type Type { get; }
        
        public ScannedAssemblyRecord(Type type, ScannedNamespace ns, ScannedType? surroundingType = null) : base(ns, type.Name, surroundingType)
        {
            Type = type;
        }
        
        protected override bool PrintMembers(StringBuilder builder)
        {
            InternalPrintMembers(builder.StartAppendingFields())
                .AppendField(nameof(Type), Type);

            return true;
        }
    }
}