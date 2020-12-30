using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.Bases;

namespace RoslynReflection.Models.FromSource
{
    internal class SourceNamespace : BaseNamespace
    {
        public override IModule Module => SourceModule;
        internal readonly SourceModule SourceModule;

        public override IEnumerable<IType> Types => SourceTypes;
        internal readonly ValueList<SourceType> SourceTypes = new();

        public SourceNamespace(SourceModule sourceModule, string name) : base(name)
        {
            SourceModule = sourceModule;
        }

        protected bool Equals(SourceNamespace other)
        {
            return base.Equals(other) && SourceTypes.Equals(other.SourceTypes);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SourceNamespace) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ SourceTypes.GetHashCode();
            }
        }

        public static bool operator ==(SourceNamespace? left, SourceNamespace? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SourceNamespace? left, SourceNamespace? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"SourceNamespace {{ {base.ToString()}, {nameof(SourceTypes)} = {SourceTypes} }}";
        }
    }
}