namespace RoslynReflection.Models
{
    /// <summary>
    /// A special type that isn't yet resolved. If you manage to see any of these,
    /// then you probably found a bug in this library
    /// </summary>
    public record UnlinkedType : ScannedType
    {
        public UnlinkedType(string name) : base(HiddenNamespace.Instance, name)
        {
        }
    }
}