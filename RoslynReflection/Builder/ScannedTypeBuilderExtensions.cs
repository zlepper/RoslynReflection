using RoslynReflection.Models;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Builder
{
    public static class ScannedTypeBuilderExtensions
    {
        public static T MakeAbstract<T>(this T type)
            where T : ICanBeAbstract
        {
            type.IsAbstract = true;
            return type;
        }

        public static T MakePartial<T>(this T type)
            where T : ICanBePartial
        {
            type.IsPartial = true;
            return type;
        }
    }
}