using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RoslynReflection.Collections
{
    public class ValueList<T> : IEnumerable<T>
    where T : notnull
    {
        private readonly List<T> _innerCollection;

        private readonly IEqualityComparer<T> _equalityComparer;

        private readonly IComparer<T> _comparer;
        public int Count => _innerCollection.Count;
        

        public ValueList(IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
        {
            _equalityComparer = equalityComparer;
            _comparer = comparer;
            _innerCollection = new();
        }
        
        public ValueList() : this(EqualityComparer<T>.Default, Comparer<T>.Default)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _innerCollection).GetEnumerator();
        }

        internal void Add(T item)
        {
            _innerCollection.Add(item);
        }

        internal void RemoveAll(Predicate<T> match)
        {
            _innerCollection.RemoveAll(match);
        }

        protected bool Equals(ValueList<T> other)
        {
            if (other.Count != Count)
            {
                return false;
            }

            var t = _innerCollection.ToImmutableHashSet(_equalityComparer);
            var o = other.ToImmutableHashSet(_equalityComparer);

            return t.SetEquals(o);
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
            return _innerCollection.Aggregate(0, (sum, item) => unchecked(sum + item.GetHashCode() * 397));
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
            return "[ " + string.Join(", ", _innerCollection.OrderBy(k => k, _comparer)) + " ]";
        }

        internal void AddRange(IEnumerable<T> items)
        {
            _innerCollection.AddRange(items);
        }

        internal void Clear()
        {
            _innerCollection.Clear();
        }

        public T this[int index]
        {
            get => _innerCollection[index];
            internal set => _innerCollection[index] = value;
        }
    }
}