using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public abstract record ScannedInterface : ScannedType, ICanBePartial
    {
        public ScannedInterface(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
        }

        public bool IsPartial { get; set; }
    }
}