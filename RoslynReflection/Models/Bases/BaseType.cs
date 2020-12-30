using RoslynReflection.Collections;

namespace RoslynReflection.Models.Bases
{
    internal abstract class BaseType : IType
    {
        public abstract IModule Module { get; }
        public abstract INamespace Namespace { get; }

        public IType? SurroundingType { get; internal set; }

        public readonly ValueList<IType> NestedTypes = new();

        public string Name { get; }

        protected BaseType(string name)
        {
            Name = name;
        }

        protected bool Equals(BaseType other)
        {
            return NestedTypes.Equals(other.NestedTypes) && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NestedTypes.GetHashCode() * 397) ^ Name.GetHashCode();
            }
        }

        public static bool operator ==(BaseType? left, BaseType? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseType? left, BaseType? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"BaseType {{ {nameof(NestedTypes)} = {NestedTypes}, IsNestedType = {this.IsNestedType()}, {nameof(Name)} = {Name} }}";
        }
    }
}