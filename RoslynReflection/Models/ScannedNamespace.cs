using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models
{
    public record ScannedNamespace : IComparable<ScannedNamespace>
    {
        public readonly ScannedModule Module;
        public readonly string Name;
        public IEnumerable<ScannedType> Types => _types;
        private readonly ValueList<ScannedType> _types = new();

        public ScannedNamespace(ScannedModule module, string name)
        {
            Module = module;
            Name = name;
            
            module.Namespaces.Add(this);
        }

        internal virtual void AddType(ScannedType type)
        {
            _types.Add(type);
        }

        public virtual bool Equals(ScannedNamespace? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Types.Equals(other.Types);
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        public bool TryGetType(string typeName, out ScannedType? type)
        {
            foreach (var t in Types)
            {
                if (t.FullName() != typeName) continue;

                type = t;
                return true;
            }

            type = null;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Types.GetHashCode();
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder
                .AppendField(nameof(Name), Name)
                .AppendField(nameof(Types), Types);
            
            return true;
        }

        internal bool IsEmpty()
        {
            return _types.Count == 0;
        }

        public int CompareTo(ScannedNamespace? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator <(ScannedNamespace? left, ScannedNamespace? right)
        {
            return Comparer<ScannedNamespace>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(ScannedNamespace? left, ScannedNamespace? right)
        {
            return Comparer<ScannedNamespace>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(ScannedNamespace? left, ScannedNamespace? right)
        {
            return Comparer<ScannedNamespace>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(ScannedNamespace? left, ScannedNamespace? right)
        {
            return Comparer<ScannedNamespace>.Default.Compare(left, right) >= 0;
        }
    }
}