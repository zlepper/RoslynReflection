using RoslynReflection.Models;

namespace RoslynReflection.Builder
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

        public IClassBuilder NewClass(string name)
        {
            return NamespaceBuilder.NewClass(name);
        }

        public INestedClassBuilder<IClassBuilder> NewInnerClass(string name)
        {
            var c = new ScannedClass(NamespaceBuilder.Namespace, name, SourceClass);
            return new NestedClassBuilder<IClassBuilder>(NamespaceBuilder, c, this);
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

        public new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(string name)
        {
            var c = new ScannedClass(NamespaceBuilder.Namespace, name, SourceClass);
            return new NestedClassBuilder<INestedClassBuilder<TClassBuilder>>(NamespaceBuilder, c, this);
        }
    }
}