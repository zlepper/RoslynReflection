using System.Collections.Generic;
using System.Collections.Immutable;

namespace RoslynReflection.Collections
{
    internal static class Extensions
    {
        internal static bool SetEquals<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
        {
            return enumerable.ToImmutableHashSet().SetEquals(other);
        }
        
    }
}