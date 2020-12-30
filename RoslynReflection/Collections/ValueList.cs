using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoslynReflection.Collections
{
    public class ValueList<T> : IEnumerable<T>
    where T : notnull
    {
        private readonly List<T> _collectionImplementation;

        public ValueList()
        {
            _collectionImplementation = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collectionImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _collectionImplementation).GetEnumerator();
        }

        public void Add(T item)
        {
            _collectionImplementation.Add(item);
        }
        
        public void AddRange(IEnumerable<T> items)
        {
            _collectionImplementation.AddRange(items);
        }

        protected bool Equals(ValueList<T> other)
        {
            return _collectionImplementation.SequenceEqual(other._collectionImplementation);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ValueList<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _collectionImplementation.Sum(item => item.GetHashCode() * 13);
            }
        }

        public static bool operator ==(ValueList<T>? left, ValueList<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueList<T>? left, ValueList<T>? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", _collectionImplementation) + "]";
        }
    }
}