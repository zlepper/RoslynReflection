using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models
{
    public abstract record ScannedType
    {
        public readonly ScannedModule Module;
        public readonly ScannedNamespace Namespace;
        public readonly string Name;

        protected ScannedType(ScannedModule module, ScannedNamespace ns, string name)
        {
            Module = module;
            Namespace = ns;
            Name = name;
        }

        public ScannedType? SurroundingType { get; internal set; }
        public readonly ValueList<ScannedType> NestedTypes = new();

        public virtual bool Equals(ScannedType? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && NestedTypes.Equals(other.NestedTypes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ NestedTypes.GetHashCode();
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Name), Name);
            builder.AppendField(nameof(NestedTypes), NestedTypes);
            
            return true;
        }
    }
}