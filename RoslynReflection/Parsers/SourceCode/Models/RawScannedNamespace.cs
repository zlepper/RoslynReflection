using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record RawScannedNamespace
    {
        internal readonly RawScannedModule Module;
        internal readonly string Name;
        internal ValueList<RawScannedType> Types = new();

        public RawScannedNamespace(string name,RawScannedModule module)
        {
            Module = module;
            Name = name;
            module.Namespaces.Add(this);
        }

        public virtual bool Equals(RawScannedNamespace? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Types.Equals(other.Types);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Types.GetHashCode();
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(Types), Types);
            
            return true;
        }
    }
}