using System;

namespace RoslynReflection.Test.TestHelpers.TestAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SampleAttribute : Attribute
    {
        public readonly string Foo;

        public SampleAttribute(string foo)
        {
            Foo = foo;
        }

        protected bool Equals(SampleAttribute other)
        {
            return base.Equals(other) && Foo == other.Foo;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SampleAttribute) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Foo);
        }

        public static bool operator ==(SampleAttribute left, SampleAttribute right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SampleAttribute left, SampleAttribute right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"SampleAttribute {{ {nameof(Foo)} = {Foo} }}";
        }
    }
}