using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    internal class ClassBuilder : IClassBuilder
    {
        private readonly ScannedClass _sourceClass;
        private readonly NamespaceBuilder _namespaceBuilder;

        internal ClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass)
        {
            _namespaceBuilder = parent;
            _sourceClass = sourceClass;
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            return _namespaceBuilder.NewNamespace(name);
        }

        public ScannedModule Finish()
        {
            return _namespaceBuilder.Finish();
        }

        public IClassBuilder NewClass(string name)
        {
            return _namespaceBuilder.NewClass(name);
        }

        public IClassBuilder NewInnerClass(string name)
        {
            var c = new ScannedClass(_namespaceBuilder.Module, _namespaceBuilder.Namespace, name)
            {
                SurroundingType = _sourceClass
            };
            _sourceClass.NestedTypes.Add(c);
            _namespaceBuilder.Namespace.Types.Add(c);
            return new ClassBuilder(_namespaceBuilder, _sourceClass);
        }
    }
}