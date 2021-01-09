using RoslynReflection.Extensions;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedInterface : ScannedType, ICanBePartial
    {
        public ScannedInterface(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsPartial { get; set; }

        public virtual bool Equals(ScannedInterface? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && IsPartial == other.IsPartial;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ IsPartial.GetHashCode();
            }
        }

        internal override StringBuilderExtensions.FieldStringBuilder InternalPrintMembers(StringBuilderExtensions.FieldStringBuilder builder)
        {
            return base.InternalPrintMembers(builder)
                .AppendField(nameof(IsPartial), IsPartial);
        }
    }
}