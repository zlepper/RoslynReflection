namespace RoslynReflection.Models
{
    public abstract record ScannedInterface : ScannedType
    {
        public ScannedInterface(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }
    }
}