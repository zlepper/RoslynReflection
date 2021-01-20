using RoslynReflection.Extensions;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public record TypeReference : IHaveSimpleRepresentation, ITypeReference
    {
        public readonly ScannedType To;

        public TypeReference(ScannedType to)
        {
            To = to;
        }

        string IHaveSimpleRepresentation.ToSimpleRepresentation()
        {
            return To.ToSimpleRepresentation();
        }
    }
}