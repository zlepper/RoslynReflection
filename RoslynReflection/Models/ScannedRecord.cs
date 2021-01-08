using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedRecord : ScannedType, ICanBeAbstract, ICanBePartial
    {
        protected ScannedRecord(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsAbstract { get; set; }
        public bool IsPartial { get; set; }
    }
}