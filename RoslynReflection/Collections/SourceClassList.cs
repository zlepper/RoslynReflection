using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Collections
{
    internal class SourceClassList
    {
        private readonly SourceModule _module;
        private readonly SourceNamespace _namespace;
        private readonly Dictionary<string, SourceClass> _classes = new();

        internal SourceClassList(SourceModule module, SourceNamespace ns)
        {
            _module = module;
            _namespace = ns;
            
            foreach (var sourceClass in _namespace.SourceTypes.OfType<SourceClass>())
            {
                _classes[sourceClass.Name] = sourceClass;
            }
        }

        internal SourceClass GetClass(string name, SourceType? surroundingType = null)
        {
            var key = name;

            if (surroundingType != null)
            {
                key = surroundingType.FullName() + "." + key;
            }
            
            if (_classes.TryGetValue(key, out var existing))
            {
                return existing;
            }

            var c = _classes[key] = new SourceClass(_module, _namespace, name)
            {
                SurroundingType = surroundingType
            };

            if (surroundingType != null)
            {
                surroundingType.NestedTypes.Add(c);
            }
            
            _namespace.SourceTypes.Add(c);
            return c;
        }
    }
}