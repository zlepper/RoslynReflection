using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public static class ScannedNamespaceExtensions
    {
        public static ScannedType AddClass(this ScannedNamespace ns, string name)
        {
            return new (name, ns);
        }
        
    }
}