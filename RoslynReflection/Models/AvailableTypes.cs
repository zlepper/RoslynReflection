﻿using System.Collections.Generic;
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
            
            return TryFromSelf(fromType, typeName, out type) || TryGetFullyQualifiedType(typeName, out type);
        }

        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        private bool TryFromSelf(ScannedType fromType, string typeName, out ScannedType? type)
        {
            if (fromType.Namespace.TryGetType(typeName, out type))
            {
                return true;
            }

            var fullInnerName = fromType.FullName() + "." + typeName;
            if (fromType.Namespace.TryGetType(fullInnerName, out type))
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