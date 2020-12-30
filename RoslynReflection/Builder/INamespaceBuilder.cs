namespace RoslynReflection.Builder
{
    public interface INamespaceBuilder : IModuleBuilder
    {
        IClassBuilder NewClass(string name);
    }
}