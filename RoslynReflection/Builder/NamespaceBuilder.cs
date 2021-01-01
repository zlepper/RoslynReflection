using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    internal class NamespaceBuilder : INamespaceBuilder
    {
        private readonly ModuleBuilder _parent;
        internal readonly ScannedNamespace Namespace;

        internal NamespaceBuilder(ModuleBuilder parent, ScannedNamespace ns)
        {
            _parent = parent;
            Namespace = ns;
        }

        internal ScannedModule Module => _parent.Module;

        public INamespaceBuilder NewNamespace(string name)
        {
            return _parent.NewNamespace(name);
        }

        public ScannedModule Finish()
        {
            return _parent.Finish();
        }

        public IClassBuilder NewClass(string name)
        {
            var sourceClass = new ScannedClass(Namespace, name);

            return new ClassBuilder(this, sourceClass);
        }
    }
}