using RoslynReflection.Models;

namespace RoslynReflection.Extensions
{
    internal static class SimpleRepresentationExtensions
    {
        internal static string NullSafeToSimpleRepresentation<T>(this T? value)
        where T: IHaveSimpleRepresentation
        {
            if (value == null)
            {
                return "null";
            }
            else
            {
                return value.ToSimpleRepresentation();
            }
        }
    }
}