using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    internal abstract class TypeList<TScannedType, TTypeDeclarationSyntax>
        where TScannedType : ScannedType
        where TTypeDeclarationSyntax : TypeDeclarationSyntax
    {
        private readonly Dictionary<string, TScannedType> _types = new();
        protected readonly ScannedNamespace Namespace;

        protected TypeList(ScannedNamespace ns)
        {
            Namespace = ns;

            foreach (var type in Namespace.Types.OfType<TScannedType>()) _types[type.Name] = type;
        }

        internal TScannedType GetType(string name, TTypeDeclarationSyntax declarationSyntax, ScannedType? surroundingType = null)
        {
            var key = name;

            if (surroundingType != null) key = surroundingType.FullName() + "." + key;

            if (_types.TryGetValue(key, out var existing)) return existing;

            var type = _types[key] = InitType(name, declarationSyntax, surroundingType);

            return type;
        }

        protected abstract TScannedType InitType(string name, TTypeDeclarationSyntax declarationSyntax,
            ScannedType? surroundingType = null);
    }
}