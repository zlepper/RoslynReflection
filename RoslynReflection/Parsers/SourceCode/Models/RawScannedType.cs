using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;

namespace RoslynReflection.Parsers.SourceCode.Models
{
    internal record RawScannedType
    {
        internal readonly List<TypeDeclarationSyntax> TypeDeclarationSyntax;
        internal readonly string Name;
        internal readonly RawScannedNamespace Namespace;
        internal readonly ValueList<IScannedUsing> Usings = new();
        internal readonly ValueList<RawScannedType> NestedTypes = new();
        internal readonly RawScannedType? SurroundingType;

        internal RawScannedModule Module => Namespace.Module;

        public RawScannedType(string name, RawScannedNamespace ns, TypeDeclarationSyntax typeDeclarationSyntax, RawScannedType? surroundingType)
        {
            Guard.AgainstNull(ns, nameof(ns));
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(typeDeclarationSyntax, nameof(typeDeclarationSyntax));
            
            Namespace = ns;
            SurroundingType = surroundingType;
            ns.Types.Add(this);
            TypeDeclarationSyntax = new List<TypeDeclarationSyntax> {typeDeclarationSyntax};
            Name = name;


            if (SurroundingType != null)
            {
                SurroundingType.NestedTypes.Add(this);
            }
        }

        public virtual bool Equals(RawScannedType? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Usings.Equals(other.Usings) && NestedTypes.Equals(other.NestedTypes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Usings.GetHashCode();
                hashCode = (hashCode * 397) ^ NestedTypes.GetHashCode();
                return hashCode;
            }
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(Usings), Usings)
                .AppendField(nameof(NestedTypes), NestedTypes);
            
            return true;
        }

        internal string FullName()
        {
            if (SurroundingType == null)
            {
                return Name;
            }

            return $"{SurroundingType.FullName()}.{Name}";
        }
        
        public string FullyQualifiedName()
        {
            return $"{Namespace.NameAsPrefix()}{FullName()}";
        }
    }
}