using System;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Builder.Assembly
{
    internal class ClassBuilder : IClassBuilder
    {
        protected readonly ScannedClass SourceClass;
        protected readonly NamespaceBuilder NamespaceBuilder;

        internal ClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass)
        {
            NamespaceBuilder = parent;
            SourceClass = sourceClass;
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            return NamespaceBuilder.NewNamespace(name);
        }

        public ScannedModule Finish()
        {
            return NamespaceBuilder.Finish();
        }

        public IClassBuilder NewClass(Type type)
        {
            return NamespaceBuilder.NewClass(type);
        }

        public IClassBuilder NewClass<T>()
        {
            return NewClass(typeof(T));
        }

        public INestedClassBuilder<IClassBuilder> NewInnerClass(Type type)
        {
            var c = new ScannedAssemblyClass(type, NamespaceBuilder.Namespace, SourceClass);
            return new NestedClassBuilder<IClassBuilder>(NamespaceBuilder, c, this);
        }

        public INestedClassBuilder<IClassBuilder> NewInnerClass<T>()
        {
            return NewInnerClass(typeof(T));
        }


        public IClassBuilder WithAttribute(object attribute)
        {
            SourceClass.Attributes.Add(attribute);
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

        public new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(Type type)
        {
            var c = new ScannedAssemblyClass(type, NamespaceBuilder.Namespace, SourceClass);
            return new NestedClassBuilder<INestedClassBuilder<TClassBuilder>>(NamespaceBuilder, c, this);
        }

        public new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass<T>()
        {
            return NewInnerClass(typeof(T));
        }
    }
}