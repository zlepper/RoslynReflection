namespace RoslynReflection.Builder.Source
{
    public interface INamespaceBuilder : IModuleBuilder
    {
        IClassBuilder NewClass(string name);
    }
}