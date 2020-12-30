using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Builder
{
    internal class NamespaceBuilder : INamespaceBuilder
    {
        private readonly SourceNamespace _namespace;
        private readonly ModuleBuilder _parent;

        internal NamespaceBuilder(ModuleBuilder parent, SourceNamespace ns)
        {
            _parent = parent;
            _namespace = ns;
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            return _parent.NewNamespace(name);
        }

        public IModule Finish()
        {
            return _parent.Finish();
        }

        public IClassBuilder NewClass(string name)
        {
            var sourceClass = new SourceClass(_parent.Module, _namespace, name);
            _namespace.SourceTypes.Add(sourceClass);

            return new ClassBuilder(this, sourceClass);
        }
    }
}