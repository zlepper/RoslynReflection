using System.Collections.Generic;
using System.Linq;

namespace RoslynReflection.Models.Bases
{
    internal abstract class BaseNamespace : INamespace
    {
        public abstract IModule Module { get; }
        public abstract IEnumerable<IType> Types { get; }

        public IEnumerable<IClass> Classes => Types.OfType<IClass>();
        public string Name { get; }

        protected BaseNamespace(string name)
        {
            Name = name;
        }

        protected bool Equals(BaseNamespace other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseNamespace) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(BaseNamespace? left, BaseNamespace? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseNamespace? left, BaseNamespace? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"BaseNamespace {{ {nameof(Name)} = {Name} }}";
        }
    }
}