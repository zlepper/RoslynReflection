using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Models.Source
{
    public record ScannedSourceClass : ScannedClass, IScannedSourceType
    {
        public TypeDeclarationSyntax DeclarationSyntax => _classDeclarationSyntax;
        private readonly ClassDeclarationSyntax _classDeclarationSyntax;
        

        public ScannedSourceClass(ClassDeclarationSyntax declarationSyntax, ScannedNamespace ns, string name,
            ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _classDeclarationSyntax = declarationSyntax;
        }

        public ScannedSourceClass(ScannedNamespace ns, string name,
            ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _classDeclarationSyntax = (ClassDeclarationSyntax) CompilationUnit()
                .WithMembers(SingletonList<MemberDeclarationSyntax>(
                    ClassDeclaration(name)
                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                ))
                .NormalizeWhitespace()
                .Members
                .First();
        }

        public virtual bool Equals(ScannedSourceClass? other)
        {
            return base.Equals(other) ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override bool PrintMembers(StringBuilder builder)
        {
            InternalPrintMembers(builder.StartAppendingFields());
            return true;
        }
    }
}