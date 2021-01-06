using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RoslynReflection.Extensions;

namespace RoslynReflection.Models
{
    internal class AvailableTypes
    {
        internal readonly Dictionary<string, ScannedNamespace> Namespaces = new();

        public void AddNamespace(ScannedNamespace ns)
        {
            if (!Namespaces.TryGetValue(ns.Name, out var existing))
            {
                existing = new ScannedNamespace(ns.Module, ns.Name);
                Namespaces[ns.Name] = existing;
            }

            existing.Types.AddRange(ns.Types);
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        internal bool TryGetType(ScannedType fromType, string typeName, out ScannedType? type)
        {
            foreach (var usingStatement in fromType.Usings)
            {
                if (usingStatement.TryGetType(typeName, this, out type))
                {
                    return true;
                }
            }
            
            // Might be a fully qualified type, so check for that also
            return TryGetFullyQualifiedType(typeName, out type);
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        internal bool TryGetFullyQualifiedType(string typeName, out ScannedType? type)
        {
            var parts = typeName.Split('.');
            if (parts.Length == 1)
            {
                type = null;
                return false;
            }

            var ns = parts.SkipLast(1).JoinToString(".");
            typeName = parts.Last();

            IScannedUsing fakeUsing = new ScannedUsing(ns);

            return fakeUsing.TryGetType(typeName, this, out type);
        }
    }
}