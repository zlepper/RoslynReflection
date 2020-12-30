namespace RoslynReflection.Models.FromSource
{
    internal class SourceClass : SourceType, IClass
    {
        public SourceClass(SourceModule module, SourceNamespace ns, string name) : base(module, ns, name)
        {
        }

        public override string ToString()
        {
            return $"SourceClass {{ {base.ToString()} }}";
        }
    }
}