using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Builder
{
    public class ModuleBuilder : IModuleBuilder
    {
        internal readonly SourceModule Module = new();
        
        public static IModuleBuilder NewBuilder()
        {
            return new ModuleBuilder();
        }

        private ModuleBuilder()
        {
            
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            var ns = new SourceNamespace(Module, name);
            Module.SourceNamespaces.Add(ns);
            return new NamespaceBuilder(this, ns);
        }

        public IModule Finish()
        {
            return Module;
        }
    }
}