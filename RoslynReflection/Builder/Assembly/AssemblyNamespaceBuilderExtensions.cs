using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Builder.Assembly
{
    public static class AssemblyNamespaceBuilderExtensions
    {
        public static ScannedAssemblyClass AddAssemblyClass<T>(this ScannedNamespace ns)
        {
            return new(typeof(T), ns);
        }
    }
}