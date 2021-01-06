using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Builder.Source
{
    internal class NamespaceBuilder : INamespaceBuilder
    {
        private readonly SourceModuleBuilder _parent;
        internal readonly ScannedNamespace Namespace;

        internal NamespaceBuilder(SourceModuleBuilder parent, ScannedNamespace ns)
        {
            _parent = parent;
            Namespace = ns;
        }

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
            var sourceClass = new ScannedSourceClass(Namespace, name);

            return new ClassBuilder(this, sourceClass);
        }
    }
}