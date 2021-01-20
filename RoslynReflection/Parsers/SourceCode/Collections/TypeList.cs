using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode.Collections
{
    internal class TypeList
    {
        private readonly Dictionary<string, RawScannedType> _types = new();
        protected readonly RawScannedNamespace Namespace;

        internal TypeList(RawScannedNamespace ns)
        {
            Namespace = ns;

            foreach (var type in Namespace.Types) _types[type.Name] = type;
        }

        internal RawScannedType GetType(TypeDeclarationSyntax declarationSyntax, RawScannedType? surroundingType = null)
        {
            var name = declarationSyntax.Identifier.ValueText.Trim();
            
            var key = name;

            if (surroundingType != null) key = surroundingType.FullName() + "." + key;

            if (_types.TryGetValue(key, out var existing))
            {
                existing.TypeDeclarationSyntax.Add(declarationSyntax);
                return existing;
            };

            var type = _types[key] = new RawScannedType(name, Namespace, declarationSyntax, surroundingType);

            return type;
        }
    }
}