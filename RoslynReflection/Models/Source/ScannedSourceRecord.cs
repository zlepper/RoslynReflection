using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Models.Source
{
    public record ScannedSourceRecord : ScannedRecord, IScannedSourceType
    {
        public ScannedSourceRecord(RecordDeclarationSyntax recordDeclaration, ScannedNamespace ns, string name,
            ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _recordDeclaration = recordDeclaration;
        }

        public ScannedSourceRecord(ScannedNamespace ns, string name, ScannedType? surroundingType = null) : base(ns, name, surroundingType)
        {
            _recordDeclaration = RecordDeclaration(Token(SyntaxKind.RecordKeyword), name);
        }

        public TypeDeclarationSyntax DeclarationSyntax => _recordDeclaration;
        private readonly RecordDeclarationSyntax _recordDeclaration;
        
        public virtual bool Equals(ScannedSourceRecord? other)
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