using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class ClassDeclarationParser
    {
        private ClassList _classList;
        private ScannedType? _surroundingType;

        public ClassDeclarationParser(ClassList classList, ScannedType? surroundingType = null)
        {
            _classList = classList;
            _surroundingType = surroundingType;
        }

        internal ScannedClass ParseClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            var name = classDeclaration.Identifier.ValueText.Trim();

            var sourceClass = _classList.GetType(name, classDeclaration, _surroundingType);

            var typeDeclarationParser = new TypeDeclarationParser(_classList, sourceClass);
            foreach (var typeDeclaration in classDeclaration.Members.OfType<TypeDeclarationSyntax>())
            {
                typeDeclarationParser.ParseTypeDeclaration(typeDeclaration);
            }
            

            
            
            return sourceClass;
        }
    }
}