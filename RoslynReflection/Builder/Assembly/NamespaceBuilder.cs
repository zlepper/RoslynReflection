using System;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;

namespace RoslynReflection.Builder.Assembly
{
    internal class NamespaceBuilder : INamespaceBuilder
    {
        private readonly AssemblyModuleBuilder _parent;
        internal readonly ScannedNamespace Namespace;

        public NamespaceBuilder(AssemblyModuleBuilder parent, ScannedNamespace ns)
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

        public IClassBuilder NewClass(Type type)
        {
            var sourceClass = new ScannedAssemblyClass(type, Namespace);

            return new ClassBuilder(this, sourceClass);
        }

        public IClassBuilder NewClass<T>()
        {
            return NewClass(typeof(T));
        }
    }
}