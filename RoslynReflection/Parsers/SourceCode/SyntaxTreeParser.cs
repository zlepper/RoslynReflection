using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Collections;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class SyntaxTreeParser
    {
        private readonly NamespaceList _namespaces;

        internal SyntaxTreeParser(RawScannedModule module)
        {
            _namespaces = new(module);
        }

        internal void ParseSyntaxTree(SyntaxTree document)
        {
            var rootUsings = UsingDeclarationParser.ExtractUsings(document.GetRoot()).ToList();


            var namespaceParser = new NamespaceDeclarationParser(_namespaces);
            
            var namespaceDeclarations = document.GetRoot()
                .ChildNodes()
                .OfType<NamespaceDeclarationSyntax>();
            
            foreach (var namespaceDeclaration in namespaceDeclarations)
            {
                var namespaces = namespaceParser.ParseNamespaceDeclaration(namespaceDeclaration);
                var usings = rootUsings.Concat(UsingDeclarationParser.ExtractUsings(namespaceDeclaration)).ToList();

                foreach (var (namespaceDeclarationSyntax, ns) in namespaces)
                {
                    var typeListList = new TypeList(ns);
                    var typeParser = new TypeDeclarationParser(typeListList, usings);
                    
                    foreach (var typeDeclarationSyntax in namespaceDeclarationSyntax.Members.OfType<TypeDeclarationSyntax>())
                    {
                        typeParser.ParseTypeDeclaration(typeDeclarationSyntax);
                    }
                }
            }
        }

    }
}