using System;
using System.Collections.Generic;

namespace RoslynReflection.Helpers
{
    internal sealed class AttributeComparer : IEqualityComparer<object>
    {
        private readonly AttributeUsageAttributeComparer _attributeUsageAttributeComparer = new();
        
        public new bool Equals(object x, object y)
        {
            return (x, y) switch
            {
                (AttributeUsageAttribute a1, AttributeUsageAttribute a2) => _attributeUsageAttributeComparer.Equals(a1,
                    a2),
                _ => x.Equals(y)
            };
        }

        public int GetHashCode(object obj)
        {
            return obj switch
            {
                AttributeUsageAttribute a => _attributeUsageAttributeComparer.GetHashCode(a),
                _ => obj.GetHashCode()
            };
        }
        
        private AttributeComparer() {}

        internal static readonly AttributeComparer Instance = new();

        private class AttributeUsageAttributeComparer : IEqualityComparer<AttributeUsageAttribute>
        {
            public bool Equals(AttributeUsageAttribute? x, AttributeUsageAttribute? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.AllowMultiple == y.AllowMultiple && x.Inherited == y.Inherited && x.ValidOn == y.ValidOn;
            }

            public int GetHashCode(AttributeUsageAttribute obj)
            {
                unchecked
                {
                    var hashCode = obj.AllowMultiple.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.Inherited.GetHashCode();
                    hashCode = (hashCode * 397) ^ (int) obj.ValidOn;
                    return hashCode;
                }
            }
        }
    }
}