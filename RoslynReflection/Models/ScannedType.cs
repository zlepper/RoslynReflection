using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models
{
    public abstract record ScannedType
    {
        public ScannedModule Module => Namespace.Module;
        public readonly ScannedNamespace Namespace;
        public readonly string Name;
        public readonly ScannedType? SurroundingType;

        protected ScannedType(ScannedNamespace ns, string name, ScannedType? surroundingType = null)
        {
            Namespace = ns;
            Name = name;
            SurroundingType = surroundingType;

            ns.Types.Add(this);

            if (surroundingType != null)
            {
                surroundingType.NestedTypes.Add(this);
            }
        }

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
            builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(NestedTypes), NestedTypes);
            
            return true;
        }
    }
}