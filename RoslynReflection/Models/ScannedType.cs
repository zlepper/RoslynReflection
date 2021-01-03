using System;
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

        public readonly ValueList<Attribute> Attributes = new();

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
            return Name == other.Name && Attributes.Equals(other.Attributes) && NestedTypes.Equals(other.NestedTypes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
                hashCode = (hashCode * 397) ^ NestedTypes.GetHashCode();
                return hashCode;
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(NestedTypes), NestedTypes)
                .AppendField(nameof(Attributes), Attributes);
            
            return true;
        }
    }
}