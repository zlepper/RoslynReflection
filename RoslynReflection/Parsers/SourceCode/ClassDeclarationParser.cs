using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class ClassDeclarationParser
    {
        private readonly ClassList _classList;
        private readonly List<IScannedUsing> _scannedUsings;
        private readonly ScannedType? _surroundingType;

        public ClassDeclarationParser(ClassList classList, List<IScannedUsing> scannedUsings,
            ScannedType? surroundingType = null)
        {
            _classList = classList;
            _scannedUsings = scannedUsings;
            _surroundingType = surroundingType;
        }

        internal ScannedClass ParseClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            var name = classDeclaration.Identifier.ValueText.Trim();

            var sourceClass = _classList.GetType(name, classDeclaration, _surroundingType);

            var typeDeclarationParser = new TypeDeclarationParser(_classList, _scannedUsings, sourceClass);
            foreach (var typeDeclaration in classDeclaration.Members.OfType<TypeDeclarationSyntax>())
            {
                typeDeclarationParser.ParseTypeDeclaration(typeDeclaration);
            }
            

            
            
            return sourceClass;
        }
    }
}