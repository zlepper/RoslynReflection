using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Models;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Collections
{
    internal abstract class SourceTypeList<T>
    where T: SourceType
    {
        protected readonly SourceModule Module;
        protected readonly SourceNamespace Namespace;
        private readonly Dictionary<string, T> _types = new();

        protected SourceTypeList(SourceModule module, SourceNamespace ns)
        {
            Module = module;
            Namespace = ns;
            
            foreach (var sourceType in Namespace.SourceTypes.OfType<T>())
            {
                _types[sourceType.Name] = sourceType;
            }
        }

        internal T GetType(string name, SourceType? surroundingType = null)
        {
            var key = name;

            if (surroundingType != null)
            {
                key = surroundingType.FullName() + "." + key;
            }
            
            if (_types.TryGetValue(key, out var existing))
            {
                return existing;
            }

            var type = _types[key] = InitType(name);
            type.SurroundingType = surroundingType;

            if (surroundingType != null)
            {
                surroundingType.NestedTypes.Add(type);
            }
            
            Namespace.SourceTypes.Add(type);
            return type;
        }

        protected abstract T InitType(string name);

    }
}