using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public static class ScannedModuleExtensions
    {
        public static ScannedNamespace AddNamespace(this ScannedModule module, string name)
        {
            return new(module, name);
        }
    }
}