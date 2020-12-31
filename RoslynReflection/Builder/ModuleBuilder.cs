using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public class ModuleBuilder : IModuleBuilder
    {
        internal readonly ScannedModule Module = new();

        private ModuleBuilder()
        {
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            var ns = new ScannedNamespace(Module, name);
            Module.Namespaces.Add(ns);
            return new NamespaceBuilder(this, ns);
        }

        public ScannedModule Finish()
        {
            return Module;
        }

        public static IModuleBuilder NewBuilder()
        {
            return new ModuleBuilder();
        }
    }
}