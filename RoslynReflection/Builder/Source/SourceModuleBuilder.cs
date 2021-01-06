using RoslynReflection.Models;

namespace RoslynReflection.Builder.Source
{
    public class SourceModuleBuilder : IModuleBuilder
    {
        internal readonly ScannedModule Module = new();

        private SourceModuleBuilder()
        {
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            var ns = new ScannedNamespace(Module, name);
            return new NamespaceBuilder(this, ns);
        }

        public ScannedModule Finish()
        {
            return Module;
        }

        public static IModuleBuilder NewBuilder()
        {
            return new SourceModuleBuilder();
        }
    }
}