using RoslynReflection.Models.Bases;

namespace RoslynReflection.Models.FromSource
{
    internal abstract class SourceType : BaseType
    {
        public override IModule Module => SourceModule;
        internal readonly SourceModule SourceModule;

        public override INamespace Namespace => SourceNamespace;
        internal readonly SourceNamespace SourceNamespace;

        protected SourceType(SourceModule module, SourceNamespace ns, string name) : base(name)
        {
            SourceModule = module;
            SourceNamespace = ns;
        }


        public override string ToString()
        {
            return $"SourceType {{ {base.ToString()} }}";
        }
    }
}