using System;
using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Builder
{
    internal class ClassBuilder : IClassBuilder
    {
        private NamespaceBuilder _namespaceBuilder;
        private readonly SourceClass _sourceClass;

        internal ClassBuilder(NamespaceBuilder parent, SourceClass sourceClass)
        {
            _namespaceBuilder = parent;
            _sourceClass = sourceClass;
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            return _namespaceBuilder.NewNamespace(name);
        }

        public IModule Finish()
        {
            return _namespaceBuilder.Finish();
        }

        public IClassBuilder NewClass(string name)
        {
            return _namespaceBuilder.NewClass(name);
        }

        public IClassBuilder NewInnerClass(string name)
        {
            var c = new SourceClass(_namespaceBuilder.Module, _namespaceBuilder.Namespace, name)
            {
                SurroundingType = _sourceClass
            };
            _sourceClass.NestedTypes.Add(c);
            _namespaceBuilder.Namespace.SourceTypes.Add(c);
            return new ClassBuilder(_namespaceBuilder, _sourceClass);
        }
    }
}