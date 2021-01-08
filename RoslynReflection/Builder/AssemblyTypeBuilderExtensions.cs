using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Builder
{
    public static class AssemblyTypeBuilderExtensions
    {
        public static ScannedAssemblyClass AddNestedAssemblyClass<T>(this ScannedType type)
        {
            return new(typeof(T), type.Namespace, type);
        }
    }
}