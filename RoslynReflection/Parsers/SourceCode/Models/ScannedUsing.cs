using System;
using System.Collections.Generic;
using RoslynReflection.Helpers;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record ScannedUsing : IScannedUsing, IComparable<ScannedUsing>
    {
        public readonly string Namespace;

        public ScannedUsing(string ns)
        {
            Namespace = ns;
        }

        bool IScannedUsing.TryGetType(string typeName, AvailableTypes availableTypes,
            out ScannedType type)
        {
            if (availableTypes.Namespaces.TryGetValue(Namespace, out var ns))
                return ns.TryGetType(typeName, out type);

            type = null!;
            return false;
        }

        public int CompareTo(ScannedUsing? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Namespace, other.Namespace, StringComparison.Ordinal);
        }

#pragma warning disable 8604
        public static bool operator <(ScannedUsing? left, ScannedUsing? right)
        {
            return Comparer<ScannedUsing>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(ScannedUsing? left, ScannedUsing? right)
        {
            return Comparer<ScannedUsing>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(ScannedUsing? left, ScannedUsing? right)
        {
            return Comparer<ScannedUsing>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(ScannedUsing? left, ScannedUsing? right)
        {
            return Comparer<ScannedUsing>.Default.Compare(left, right) >= 0;
        }
#pragma warning restore 8604
    }
}