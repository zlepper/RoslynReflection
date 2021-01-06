using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoslynReflection.Extensions;

namespace RoslynReflection.Collections
{
    public class ValueDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
    {
        private readonly Dictionary<TKey, TValue> _innerDict = new();

        protected bool Equals(ValueDictionary<TKey, TValue> other)
        {
            return _innerDict.SequenceEqual(other._innerDict);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValueDictionary<TKey, TValue>) obj);
        }

        public override int GetHashCode()
        {
            return _innerDict.GetHashCode();
        }

        public static bool operator ==(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueDictionary<TKey, TValue>? left, ValueDictionary<TKey, TValue>? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("ValueDictionary {");

            if (_innerDict.Count != 0)
            {
                var first = _innerDict.First();

                var remaining = _innerDict.Skip(1);

                var builder = sb.AppendField(first.Key.ToString(), first.Value.ToString());

                foreach (var pair in remaining)
                {
                    builder.AppendField(pair.Key.ToString(), pair.Value);
                }
            }

            sb.Append(" }");
            return $"ValueDictionary {{ {nameof(_innerDict)}: {_innerDict} }}";
        }
    }
}