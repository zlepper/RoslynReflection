using System.Linq;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;

namespace RoslynReflection.Models
{
    public record ScannedUsingAlias : IScannedUsing
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
    }
}