using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Builder
{
    public static class AssemblyNamespaceBuilderExtensions
    {
        public static ScannedAssemblyClass AddAssemblyClass<T>(this ScannedNamespace ns)
        {
            return new(typeof(T), ns);
        }

        public static ScannedAssemblyInterface AddAssemblyInterface<T>(this ScannedNamespace ns)
        {
            return new(typeof(T), ns);
        }
    }
}