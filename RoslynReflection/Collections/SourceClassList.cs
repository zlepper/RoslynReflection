using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Collections
{
    internal class SourceClassList : SourceTypeList<SourceClass>
    {
        public SourceClassList(SourceModule module, SourceNamespace ns) : base(module, ns)
        {
        }

        protected override SourceClass InitType(string name)
        {
            return new(Module, Namespace, name);
        }
    }
}