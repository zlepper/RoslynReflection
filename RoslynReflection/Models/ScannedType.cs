using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedType : IScannedType, ICanNavigateToModule
    {
        public ScannedModule Module => Namespace.Module;
        public ScannedNamespace Namespace { get; }
        public string Name { get; }
        public ScannedType? SurroundingType { get; }

        public ValueList<ScannedType> NestedTypes { get; } = new();
        public ValueList<object> Attributes { get; } = new(AttributeComparer.Instance);

        public ValueList<IScannedUsing> Usings { get; } = new();

        internal ValueList<ScannedType> BaseTypes { get; } = new();

        public ValueList<ScannedInterface> ImplementedInterfaces { get; } = new();

        protected ScannedType(ScannedNamespace ns, string name, ScannedType? surroundingType = null)
        {
            Namespace = ns;
            Name = name;
            SurroundingType = surroundingType;

            ns.AddType(this);

            if (surroundingType != null)
            {
                surroundingType.NestedTypes.Add(this);
            }
        }

        public virtual bool Equals(ScannedType? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && 
                   Attributes.Equals(other.Attributes) && 
                   NestedTypes.Equals(other.NestedTypes) &&
                   Usings.Equals(other.Usings) && 
                   BaseTypes.Equals(other.BaseTypes) &&
                   ImplementedInterfaces.Equals(other.ImplementedInterfaces);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
                hashCode = (hashCode * 397) ^ NestedTypes.GetHashCode();
                hashCode = (hashCode * 397) ^ Usings.GetHashCode();
                hashCode = (hashCode * 397) ^ BaseTypes.GetHashCode();
                hashCode = (hashCode * 397) ^ ImplementedInterfaces.GetHashCode();
                return hashCode;
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            InternalPrintMembers(builder.StartAppendingFields());

            return true;
        }

        internal virtual StringBuilderExtensions.FieldStringBuilder InternalPrintMembers(
            StringBuilderExtensions.FieldStringBuilder builder)
        {
            return builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(NestedTypes), NestedTypes)
                .AppendField(nameof(Attributes), Attributes)
                .AppendField(nameof(Usings), Usings)
                .AppendField(nameof(BaseTypes), BaseTypes)
                .AppendField(nameof(ImplementedInterfaces), ImplementedInterfaces);
        }
    }
}