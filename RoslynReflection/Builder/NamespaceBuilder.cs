using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Builder
{
    internal class NamespaceBuilder : INamespaceBuilder
    {
        internal readonly SourceNamespace Namespace;
        private readonly ModuleBuilder _parent;

        internal NamespaceBuilder(ModuleBuilder parent, SourceNamespace ns)
        {
            _parent = parent;
            Namespace = ns;
        }

        internal SourceModule Module => _parent.Module;

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
            var sourceClass = new SourceClass(_parent.Module, Namespace, name);
            Namespace.SourceTypes.Add(sourceClass);

            return new ClassBuilder(this, sourceClass);
        }
    }
}