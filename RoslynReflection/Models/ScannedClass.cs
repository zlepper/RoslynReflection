namespace RoslynReflection.Models
{
    public record ScannedClass : ScannedType
    {
        public ScannedClass(ScannedModule module, ScannedNamespace ns, string name) : base(module, ns, name)
        {
        }
    }
}