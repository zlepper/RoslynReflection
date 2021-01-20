
namespace RoslynReflection.Extensions
{
    internal static class ObjectExtensions
    {
        internal static bool NullSafeEquals<T>(this T? obj, T? other)
        where T : notnull
        {
            if (ReferenceEquals(obj, other)) return true;
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(other, null)) return false;
            return obj.Equals(other);
        }
    }
}