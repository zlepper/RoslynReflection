using System;
using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record ScannedUsingAlias : IScannedUsing, IComparable<ScannedUsingAlias>
    {
        public readonly string Import;
        public readonly string Alias;

        public ScannedUsingAlias(string import, string alias)
        {
            Import = import;
            Alias = alias;
        }

        bool IScannedUsing.TryGetType(string typeName, AvailableTypes availableTypes,
            out ScannedType? type)
        {
            type = null;
            var parts = typeName.Split('.');
            if (parts.Length == 1)
                return false;

            if (parts[0] != Alias)
                return false;

            // Safe to ignore the null, since we cannot get an empty string in this context (At least i don't believe we can)
            typeName = parts.Last()!;
            
            var additionalNamespaceSections = parts.Skip(1).SkipLast(1).JoinToString(".");
            
            var fullImport = Import;
            if (additionalNamespaceSections != "")
            {
                fullImport += "." + additionalNamespaceSections;
            }

            IScannedUsing normalUsing = new ScannedUsing(fullImport);

            return normalUsing.TryGetType(typeName, availableTypes, out type);
        }

        public int CompareTo(ScannedUsingAlias? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var importComparison = string.Compare(Import, other.Import, StringComparison.Ordinal);
            if (importComparison != 0) return importComparison;
            return string.Compare(Alias, other.Alias, StringComparison.Ordinal);
        }

#pragma warning disable 8604
        public static bool operator <(ScannedUsingAlias? left, ScannedUsingAlias? right)
        {
            return Comparer<ScannedUsingAlias>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(ScannedUsingAlias? left, ScannedUsingAlias? right)
        {
            return Comparer<ScannedUsingAlias>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(ScannedUsingAlias? left, ScannedUsingAlias? right)
        {
            return Comparer<ScannedUsingAlias>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(ScannedUsingAlias? left, ScannedUsingAlias? right)
        {
            return Comparer<ScannedUsingAlias>.Default.Compare(left, right) >= 0;
        }
#pragma warning restore 8604
    }
}