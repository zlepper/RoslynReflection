using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    internal class ClassList : SourceTypeList<ScannedClass>
    {
        public ClassList(ScannedModule module, ScannedNamespace ns) : base(module, ns)
        {
        }

        protected override ScannedClass InitType(string name)
        {
            return new(Module, Namespace, name);
        }
    }
}