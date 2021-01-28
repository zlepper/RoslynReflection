using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n),
                    "Argument n should be non-negative.");

            return InternalSkipLast(source, n);
        }

        private static IEnumerable<T> InternalSkipLast<T>(IEnumerable<T> source, int n)
        {
            Queue<T> buffer = new(n + 1);

            foreach (var x in source)
            {
                buffer.Enqueue(x);

                if (buffer.Count == n + 1)
                    yield return buffer.Dequeue();
            }
        }

        internal static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        internal static string ToSimpleRepresentation<T>(this IEnumerable<T> enumerable)
            where T : IHaveSimpleRepresentation
        {
            return "[ " + enumerable.Select(v => v.ToSimpleRepresentation()).JoinToString(", ") + " ]";
        }

        internal static ValueList<T> ToValueList<T>(this IEnumerable<T> enumerable)
        where T : notnull
        {
            var list = new ValueList<T>();
            list.AddRange(enumerable);
            return list;
        }

        internal static IEnumerable<(TValue value, int index)> WithIndex<TValue>(this IEnumerable<TValue> enumerable)
        {
            return enumerable.Select((v, i) => (v, i));
        }
    }
}