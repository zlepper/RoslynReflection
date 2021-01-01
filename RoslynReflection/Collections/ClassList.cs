using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    internal class ClassList : TypeList<ScannedClass>
    {
        public ClassList(ScannedModule module, ScannedNamespace ns) : base(module, ns)
        {
        }

        protected override ScannedClass InitType(string name, ScannedType? surroundingType = null)
        {
            return new(Namespace, name, surroundingType);
        }
    }
}