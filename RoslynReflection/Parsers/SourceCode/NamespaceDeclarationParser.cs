using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class NamespaceDeclarationParser
    {
        private SourceNamespaceList _namespaceList;

        internal NamespaceDeclarationParser(SourceNamespaceList namespaceList)
        {
            _namespaceList = namespaceList;
        }

        internal IEnumerable<(NamespaceDeclarationSyntax declaration, ScannedNamespace ns)> ParseNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration, ScannedNamespace? parentNamespace = null)
        {
            var name = namespaceDeclaration.Name.GetText().ToString().Trim();

            if (parentNamespace != null)
            {
                name = $"{parentNamespace.Name}.{name}";
            }

            var ns = _namespaceList.GetNamespace(name);

            var childNamespaces = namespaceDeclaration
                .DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .SelectMany(s => ParseNamespaceDeclaration(s, ns));

            return Enumerable.Repeat((namespaceDeclaration, ns), 1).Concat(childNamespaces);
        }
    }
}