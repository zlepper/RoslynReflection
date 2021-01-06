using System;
using System.Collections.Generic;

namespace RoslynReflection.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

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
    }
}