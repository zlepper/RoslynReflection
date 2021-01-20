using RoslynReflection.Collections;

namespace RoslynReflection.Models
{
    public record GenericTypeReference : TypeReference
    {
        public readonly ValueList<ITypeReference> GenericArguments;
        
        public GenericTypeReference(ScannedType to, ValueList<ITypeReference> genericArguments) : base(to)
        {
            GenericArguments = genericArguments;
        }
    }
}