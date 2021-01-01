using System;
using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    internal class ClassBuilder : IClassBuilder
    {
        private readonly ScannedClass _sourceClass;
        private readonly NamespaceBuilder _namespaceBuilder;
        private readonly ClassBuilder? _parentClassBuilder;

        internal ClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass)
        {
            _namespaceBuilder = parent;
            _sourceClass = sourceClass;
        }

        private ClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass, ClassBuilder parentClassBuilder) : this(parent, sourceClass)
        {
            _parentClassBuilder = parentClassBuilder;
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
            var c = new ScannedClass(_namespaceBuilder.Namespace, name, _sourceClass);
            return new ClassBuilder(_namespaceBuilder, _sourceClass, this);
        }

        public IClassBuilder GoBackToParent()
        {
            if (_parentClassBuilder == null)
            {
                throw new InvalidOperationException("Currently not interacting with a nested class");
            }

            return _parentClassBuilder;
        }
    }
}