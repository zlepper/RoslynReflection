using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Collections
{
    internal class SourceClassList
    {
        private SourceModule _module;
        private SourceNamespace _namespace;
        private Dictionary<string, SourceClass> _classes = new();

        internal SourceClassList(SourceModule module, SourceNamespace ns)
        {
            _module = module;
            _namespace = ns;
            
            foreach (var sourceClass in _namespace.SourceTypes.OfType<SourceClass>())
            {
                _classes[sourceClass.Name] = sourceClass;
            }
        }

        internal SourceClass GetClass(string name)
        {
            if (_classes.TryGetValue(name, out var existing))
            {
                return existing;
            }
            
            var c = _classes[name] = new SourceClass(_module, _namespace, name);
            _namespace.SourceTypes.Add(c);
            return c;
        }
    }
}