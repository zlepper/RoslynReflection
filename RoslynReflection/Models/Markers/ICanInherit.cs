namespace RoslynReflection.Models.Markers
{
    public interface ICanInherit : IScannedType
    {
        public ITypeReference? ParentType { get; set; }
    }
}