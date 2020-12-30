namespace RoslynReflection.Models
{
    public interface IType
    {
        IModule Module { get; }
        INamespace Namespace { get; }
        string Name { get; }

        IType? SurroundingType { get; }
    }
}