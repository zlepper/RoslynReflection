using System.Collections.Generic;

namespace RoslynReflection.Models.Markers
{
    public interface ICanInherit : ICanNavigateToModule
    {
        public ScannedType? ParentType { get; set; }
    }
}