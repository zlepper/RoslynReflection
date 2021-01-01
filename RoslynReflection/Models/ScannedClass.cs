namespace RoslynReflection.Models
{
    public record ScannedClass : ScannedType
    {
        public ScannedClass(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }
    }
}