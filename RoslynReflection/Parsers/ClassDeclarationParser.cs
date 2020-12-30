using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers
{
    internal class ClassDeclarationParser
    {
        private SourceClassList _classList;

        public ClassDeclarationParser(SourceClassList classList)
        {
            _classList = classList;
        }

        internal SourceClass ParseClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            var name = classDeclaration.Identifier.ValueText.Trim();

            return _classList.GetClass(name);
        }
    }
}