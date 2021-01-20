namespace RoslynReflection.Models
{
    public record GenericArgumentReference : ITypeReference
    {
        public readonly string Name;

        public GenericArgumentReference(string name)
        {
            Name = name;
        }

        public string ToSimpleRepresentation()
        {
            return Name;
        }
    }
}