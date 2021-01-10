using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public static class SourceModuleBuilderExtensions
    {
        public static ScannedNamespace AddNamespace(this ScannedModule module, string name)
        {
            return new(module, name);
        }

        public static ScannedModule AddDependency(this ScannedModule module, ScannedModule other)
        {
            module.DependsOn.Add(other);
            module.DependsOn.AddRange(other.GetAllDependencies());
            return module;
        }
    }
}