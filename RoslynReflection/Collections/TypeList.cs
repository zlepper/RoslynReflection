using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    internal abstract class TypeList<T>
        where T : ScannedType
    {
        private readonly Dictionary<string, T> _types = new();
        protected readonly ScannedModule Module;
        protected readonly ScannedNamespace Namespace;

        protected TypeList(ScannedModule module, ScannedNamespace ns)
        {
            Module = module;
            Namespace = ns;

            foreach (var type in Namespace.Types.OfType<T>()) _types[type.Name] = type;
        }

        internal T GetType(string name, ScannedType? surroundingType = null)
        {
            var key = name;

            if (surroundingType != null) key = surroundingType.FullName() + "." + key;

            if (_types.TryGetValue(key, out var existing)) return existing;

            var type = _types[key] = InitType(name, surroundingType);

            return type;
        }

        protected abstract T InitType(string name, ScannedType? surroundingType = null);
    }
}