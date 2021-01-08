using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal abstract class BaseConcreteTypeDeclarationParser<TType, TDeclarationSyntax>
    where TType : ScannedType
    where TDeclarationSyntax : TypeDeclarationSyntax
    {
        protected readonly TypeListList TypeListList;
        private readonly List<IScannedUsing> _scannedUsings;
        private readonly ScannedType? _surroundingType;

        public BaseConcreteTypeDeclarationParser(TypeListList typeListList, List<IScannedUsing> scannedUsings, ScannedType? surroundingType)
        {
            TypeListList = typeListList;
            _scannedUsings = scannedUsings;
            _surroundingType = surroundingType;
        }

        protected abstract TypeList<TType, TDeclarationSyntax> TypeList { get; }

        internal TType ParseDeclaration(TDeclarationSyntax specificTypeDeclaration)
        {
            var name = specificTypeDeclaration.Identifier.ValueText.Trim();

            var sourceClass = TypeList.GetType(name, specificTypeDeclaration, _surroundingType);

            var typeDeclarationParser = new TypeDeclarationParser(TypeListList, _scannedUsings, sourceClass);
            foreach (var typeDeclaration in specificTypeDeclaration.Members.OfType<TypeDeclarationSyntax>())
            {
                typeDeclarationParser.ParseTypeDeclaration(typeDeclaration);
            }
            
            ModifySpecifics(specificTypeDeclaration, sourceClass);

            return sourceClass;
        }
        
        protected virtual void ModifySpecifics(TDeclarationSyntax typeDeclaration, TType type) {}
    }
}