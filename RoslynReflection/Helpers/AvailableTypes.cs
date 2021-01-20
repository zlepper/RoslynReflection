using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RoslynReflection.Extensions;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Helpers
{
    internal class AvailableTypes
    {
        internal readonly Dictionary<string, ScannedNamespace> Namespaces = new();
        private readonly ScannedModule _fakeModule = new();

        internal IEnumerable<ScannedType> Types => Namespaces.Values.SelectMany(n => n.Types);

        public AvailableTypes()
        {
            
        }

        public AvailableTypes(ScannedType type)
        {
            AddNamespaces(type.Module.Namespaces);
        }
        
        public void AddNamespace(ScannedNamespace ns)
        {
            if (!Namespaces.TryGetValue(ns.Name, out var existing))
            {
                existing = new ScannedNamespace(_fakeModule, ns.Name);
                Namespaces[ns.Name] = existing;
            }

            existing.AddTypes(ns.Types);
        }

        public void AddNamespaces(IEnumerable<ScannedNamespace> namespaces)
        {
            foreach (var ns in namespaces)
            {
                AddNamespace(ns);
            }
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        internal bool TryGetType(RawScannedType fromType, string typeName, out ScannedType? type)
        {
            foreach (var usingStatement in fromType.Usings)
            {
                if (usingStatement.TryGetType(typeName, this, out type))
                {
                    return true;
                }
            }
            
            return TryFromSelf(fromType, typeName, out type) || TryGetFullyQualifiedType(typeName, out type);
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        private bool TryFromSelf(RawScannedType fromType, string typeName, out ScannedType? type)
        {
            var ns = Namespaces[fromType.Namespace.Name];
            if (ns.TryGetType(typeName, out type))
            {
                return true;
            }

            var fullInnerName = fromType.FullName() + "." + typeName;
            if (ns.TryGetType(fullInnerName, out type))
            {
                return true;
            }

            type = null;
            return false;
            
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        private bool TryGetFullyQualifiedType(string typeName, out ScannedType? type)
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