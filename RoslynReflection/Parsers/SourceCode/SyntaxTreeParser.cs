using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class SyntaxTreeParser
    {
        private readonly ScannedModule _module;
        private NamespaceList _namespaces;
        
        internal SyntaxTreeParser(ScannedModule module)
        {
            _module = module;
            _namespaces = new NamespaceList(module);
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

                foreach (var (namespaceDeclarationSyntax, ns) in namespaces)
                {
                    var classList = new ClassList(ns);
                    var typeParser = new TypeDeclarationParser(classList);
                    
                    foreach (var typeDeclarationSyntax in namespaceDeclarationSyntax.Members.OfType<TypeDeclarationSyntax>())
                    {
                        typeParser.ParseTypeDeclaration(typeDeclarationSyntax);
                    }
                }
            }
        }
    }
}