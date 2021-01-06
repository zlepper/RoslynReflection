using System;
using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    internal class ClassBuilder : IClassBuilder
    {
        protected readonly ScannedClass _sourceClass;
        protected readonly NamespaceBuilder _namespaceBuilder;

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

        public INestedClassBuilder<IClassBuilder> NewInnerClass(string name)
        {
            var c = new ScannedClass(_namespaceBuilder.Namespace, name, _sourceClass);
            return new NestedClassBuilder<IClassBuilder>(_namespaceBuilder, c, this);
        }


        public IClassBuilder WithAttribute(object attribute)
        {
            _sourceClass.Attributes.Add(attribute);
            return this;
        }
    }

    internal class NestedClassBuilder<TClassBuilder> : ClassBuilder, INestedClassBuilder<TClassBuilder>
        where TClassBuilder : IClassBuilder
    {
        
        private readonly TClassBuilder _parentClassBuilder;

        internal NestedClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass, TClassBuilder parentClassBuilder) : base(parent, sourceClass)
        {
            _parentClassBuilder = parentClassBuilder;
        }

        public TClassBuilder GoBackToParent()
        {
            return _parentClassBuilder;
        }

        public new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(string name)
        {
            var c = new ScannedClass(_namespaceBuilder.Namespace, name, _sourceClass);
            return new NestedClassBuilder<INestedClassBuilder<TClassBuilder>>(_namespaceBuilder, c, this);
        }
    }
}