namespace RoslynReflection.Models.Bases
{
    internal abstract class BaseType : IType
    {
        public abstract IModule Module { get; }
        public abstract INamespace Namespace { get; }

        public string Name { get; }

        protected BaseType(string name)
        {
            Name = name;
        }

        protected bool Equals(BaseType other)
        {
            return Name == other.Name;
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
            return Name.GetHashCode();
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
            return $"BaseType {{ {nameof(Name)} = {Name} }}";
        }
    }
}