using RoslynReflection.Models;

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
    }
}