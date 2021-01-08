using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Models.Source
{
    public record ScannedSourceInterface : ScannedInterface, IScannedSourceType
    {
        public TypeDeclarationSyntax DeclarationSyntax => _interfaceDeclaration;
        private readonly InterfaceDeclarationSyntax _interfaceDeclaration;
        
        public ScannedSourceInterface(InterfaceDeclarationSyntax interfaceDeclaration, ScannedNamespace ns, string name,
            ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _interfaceDeclaration = interfaceDeclaration;
        }

        public ScannedSourceInterface(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _interfaceDeclaration = InterfaceDeclaration(name);
        }


        public virtual bool Equals(ScannedSourceInterface? other)
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