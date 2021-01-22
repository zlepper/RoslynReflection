using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;
using RoslynReflection.Models;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record RawScannedModule : IHaveDependencies
    {
        internal readonly ValueList<RawScannedNamespace> Namespaces = new();
        
        
        public ValueList<ScannedModule> DependsOn { get; } = new();

        public virtual bool Equals(RawScannedModule? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Namespaces.Equals(other.Namespaces);
        }

        public override int GetHashCode()
        {
            return Namespaces.GetHashCode();
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Namespaces), Namespaces);
            
            return true;
        }
    }
}