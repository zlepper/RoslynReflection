using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Builder.Source
{
    internal class ClassBuilder : IClassBuilder
    {
        protected readonly NamespaceBuilder NamespaceBuilder;
        protected readonly ScannedClass SourceClass;

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

        public IClassBuilder NewClass(string name)
        {
            return NamespaceBuilder.NewClass(name);
        }

        public INestedClassBuilder<IClassBuilder> NewInnerClass(string name)
        {
            var c = new ScannedSourceClass(NamespaceBuilder.Namespace, name, SourceClass);
            return new NestedClassBuilder<IClassBuilder>(NamespaceBuilder, c, this);
        }


        public IClassBuilder WithAttribute(object attribute)
        {
            SourceClass.Attributes.Add(attribute);
            return this;
        }

        public IClassBuilder WithUsing(string ns)
        {
            SourceClass.Usings.Add(new ScannedUsing(ns));
            return this;
        }

        public IClassBuilder WithAliasUsing(string ns, string alias)
        {
            SourceClass.Usings.Add(new ScannedUsingAlias(ns, alias));
            return this;
        }
    }

    internal class NestedClassBuilder<TClassBuilder> : ClassBuilder, INestedClassBuilder<TClassBuilder>
        where TClassBuilder : IClassBuilder
    {
        private readonly TClassBuilder _parentClassBuilder;

        internal NestedClassBuilder(NamespaceBuilder parent, ScannedClass sourceClass, TClassBuilder parentClassBuilder)
            : base(parent, sourceClass)
        {
            _parentClassBuilder = parentClassBuilder;
        }

        public TClassBuilder GoBackToParent()
        {
            return _parentClassBuilder;
        }

        public new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(string name)
        {
            var c = new ScannedSourceClass(NamespaceBuilder.Namespace, name, SourceClass);
            return new NestedClassBuilder<INestedClassBuilder<TClassBuilder>>(NamespaceBuilder, c, this);
        }

        public new INestedClassBuilder<TClassBuilder> WithAttribute(object attribute)
        {
            base.WithAttribute(attribute);
            return this;
        }

        public new INestedClassBuilder<TClassBuilder> WithUsing(string ns)
        {
            base.WithUsing(ns);
            return this;
        }

        public new INestedClassBuilder<TClassBuilder> WithAliasUsing(string ns, string alias)
        {
            base.WithAliasUsing(ns, alias);
            return this;
        }
    }
}