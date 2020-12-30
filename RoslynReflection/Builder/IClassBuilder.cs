namespace RoslynReflection.Builder
{
    public interface IClassBuilder : INamespaceBuilder
    {
        IClassBuilder NewInnerClass(string name);
    }
}