namespace RoslynReflection.Models
{
    public abstract record ScannedClass : ScannedType
    {
        protected ScannedClass(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }
    }
}