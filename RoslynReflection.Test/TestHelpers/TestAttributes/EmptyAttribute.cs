using System;

namespace RoslynReflection.Test.TestHelpers.TestAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EmptyAttribute : Attribute
    {
        protected bool Equals(EmptyAttribute other)
        {
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmptyAttribute) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode We need equality and i don't want to listen 
            // to resharper complain
            return base.GetHashCode();
        }

        public static bool operator ==(EmptyAttribute? left, EmptyAttribute? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmptyAttribute? left, EmptyAttribute? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "EmptyAttribute { }";
        }
    }
}