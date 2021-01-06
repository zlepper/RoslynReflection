using System;

namespace RoslynReflection.Builder.Assembly
{
    public interface INamespaceBuilder : IModuleBuilder
    {
        IClassBuilder NewClass(Type type);
        IClassBuilder NewClass<T>();
    }
}