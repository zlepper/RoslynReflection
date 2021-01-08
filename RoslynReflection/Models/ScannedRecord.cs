namespace RoslynReflection.Models
{
    public abstract record ScannedRecord : ScannedType, ICanBeAbstract
    {
        protected ScannedRecord(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsAbstract { get; set; }
    }
}