using RoslynReflection.Extensions;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedClass : ScannedType, ICanBeAbstract, ICanBePartial, ICanInherit
    {
        public bool IsAbstract { get; set; }
        
        protected ScannedClass(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsPartial { get; set; }
        public ITypeReference? ParentType { get; set; }

        public virtual bool Equals(ScannedClass? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && IsAbstract == other.IsAbstract && IsPartial == other.IsPartial && Equals(ParentType, other.ParentType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ IsAbstract.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPartial.GetHashCode();
                hashCode = (hashCode * 397) ^ (ParentType != null ? ParentType.GetHashCode() : 0);
                return hashCode;
            }
        }

        internal override StringBuilderExtensions.FieldStringBuilder InternalPrintMembers(StringBuilderExtensions.FieldStringBuilder builder)
        {
            return base.InternalPrintMembers(builder)
                .AppendField(nameof(IsAbstract), IsAbstract)
                .AppendField(nameof(IsPartial), IsPartial)
                .AppendField(nameof(ParentType), ParentType.ToSimpleRepresentation());
        }
    }
}