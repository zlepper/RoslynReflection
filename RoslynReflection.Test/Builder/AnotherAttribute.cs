using System;

namespace RoslynReflection.Test.Builder
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnotherAttribute : Attribute
    {
        public readonly string Bar;

        public AnotherAttribute(string bar)
        {
            Bar = bar;
        }

        protected bool Equals(AnotherAttribute other)
        {
            return base.Equals(other) && Bar == other.Bar;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AnotherAttribute) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Bar);
        }

        public static bool operator ==(AnotherAttribute left, AnotherAttribute right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnotherAttribute left, AnotherAttribute right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(AnotherAttribute)} {{ {nameof(Bar)} = {Bar}}}";
        }
    }
}