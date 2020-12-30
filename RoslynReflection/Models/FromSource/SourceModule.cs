using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Collections;
using RoslynReflection.Models.Bases;

namespace RoslynReflection.Models.FromSource
{
    internal class  SourceModule : BaseModule
    {
        public override IEnumerable<INamespace> Namespaces => SourceNamespaces;
        internal readonly ValueList<SourceNamespace> SourceNamespaces = new();

        protected bool Equals(SourceModule other)
        {
            return SourceNamespaces.Equals(other.SourceNamespaces);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SourceModule) obj);
        }

        public override int GetHashCode()
        {
            return SourceNamespaces.GetHashCode();
        }

        public static bool operator ==(SourceModule? left, SourceModule? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SourceModule? left, SourceModule? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"SourceModule {{ {nameof(SourceNamespaces)} = {SourceNamespaces} }}";
        }
    }
}