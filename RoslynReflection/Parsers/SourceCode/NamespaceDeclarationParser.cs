using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Collections;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class NamespaceDeclarationParser
    {
        private NamespaceList _namespaceList;

        internal NamespaceDeclarationParser(NamespaceList namespaceList)
        {
            _namespaceList = namespaceList;
        }

        internal IEnumerable<(NamespaceDeclarationSyntax declaration, RawScannedNamespace ns)> ParseNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration, RawScannedNamespace? parentNamespace = null)
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