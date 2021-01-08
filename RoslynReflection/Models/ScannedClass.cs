using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedClass : ScannedType, ICanBeAbstract, ICanBePartial
    {
        public bool IsAbstract { get; set; }
        
        protected ScannedClass(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsPartial { get; set; }
    }
}