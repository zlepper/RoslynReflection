namespace RoslynReflection.Models
{
    public abstract record ScannedClass : ScannedType, ICanBeAbstract
    {
        public bool IsAbstract { get; set; }
        
        protected ScannedClass(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }
    }
}