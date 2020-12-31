using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class ClassDeclarationParser
    {
        private SourceClassList _classList;
        private SourceType? _surroundingType;

        public ClassDeclarationParser(SourceClassList classList, SourceType? surroundingType = null)
        {
            _classList = classList;
            _surroundingType = surroundingType;
        }

        internal SourceClass ParseClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            var name = classDeclaration.Identifier.ValueText.Trim();

            var sourceClass = _classList.GetType(name, _surroundingType);

            var typeDeclarationParser = new TypeDeclarationParser(_classList, sourceClass);
            foreach (var typeDeclaration in classDeclaration.Members.OfType<TypeDeclarationSyntax>())
            {
                typeDeclarationParser.ParseTypeDeclaration(typeDeclaration);
            }
            
            
            return sourceClass;
        }
    }
}