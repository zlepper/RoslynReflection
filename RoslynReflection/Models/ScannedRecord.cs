namespace RoslynReflection.Models
{
    public abstract record ScannedRecord : ScannedType
    {
        protected ScannedRecord(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }
    }
}