using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers
{
    internal class SyntaxTreeParser
    {
        private SourceNamespaceList _namespaces;
        
        internal SyntaxTreeParser(SourceModule module)
        {
            _namespaces = new SourceNamespaceList(module);
        }

        internal void ParseSyntaxTree(SyntaxTree document)
        {
            var namespaceParser = new NamespaceDeclarationParser(_namespaces);
            
            var namespaceDeclarations = document.GetRoot()
                .ChildNodes()
                .OfType<NamespaceDeclarationSyntax>();
            
            foreach (var namespaceDeclaration in namespaceDeclarations)
            {
                var namespaces = namespaceParser.ParseNamespaceDeclaration(namespaceDeclaration);

                foreach (var namespaceDeclarationSyntax in namespaces)
                {
                    Console.WriteLine("Something");
                }
            }
        }
    }
}