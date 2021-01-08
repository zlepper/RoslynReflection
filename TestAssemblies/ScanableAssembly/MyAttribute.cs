using System;

namespace ScanableAssembly
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MyAttribute : Attribute
    {
        public readonly string MyValue;

        public MyAttribute(string myValue)
        {
            MyValue = myValue;
        }

        protected bool Equals(MyAttribute other)
        {
            return base.Equals(other) && MyValue == other.MyValue;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyAttribute) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), MyValue);
        }

        public static bool operator ==(MyAttribute left, MyAttribute right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MyAttribute left, MyAttribute right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"MyAttribute {{ {nameof(MyValue)} = {MyValue} }}";
        }
    }
}