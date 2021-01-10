using RoslynReflection.Models;

namespace RoslynReflection.Test.TestHelpers
{
    public static class ModuleBuilderExtensions
    {
        public static ScannedModule AddSingleDependency(this ScannedModule module, ScannedModule other)
        {
            module.DependsOn.Add(other);
            return module;
        }
    }
}