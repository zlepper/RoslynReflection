using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;
using RoslynReflection.Helpers;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Models
{
    public record ScannedType : IHaveSimpleRepresentation, IComparable<ScannedType>
    {
        public ScannedModule Module => Namespace.Module;

        public ScannedNamespace Namespace { get; }
        public string Name { get; }

        public ScannedType? DeclaringType { get; }
        public ValueList<object> Attributes { get; } = new(AttributeComparer.Instance, AttributeComparer.Instance);
        public ScannedType? BaseType { get; internal set; }
        public ValueList<ScannedType> ImplementedInterfaces { get; } = new();
        public ValueList<ScannedType> NestedTypes { get; } = new();

        public bool IsClass { get; internal set; }
        public bool IsInterface { get; internal set; }
        public bool IsRecord { get; internal set; }
        
        public bool IsPartial { get; internal set; }
        
        public bool IsAbstract { get; internal set; }
        
        public bool IsSealed { get; internal set; }
        
        public bool ContainsGenericParameters { get; internal set; }
        public bool IsConstructedGenericType { get; internal set; }
        public bool IsGenericType { get; internal set; }
        public bool IsGenericTypeDefinition { get; internal set; }

        public ValueList<ScannedType> GenericTypeArguments { get; } = new();
        public ValueList<ScannedType> GenericTypeParameters { get; } = new();
        
        public bool IsGenericParameter { get; internal set; }
        public int GenericParameterPosition { get; internal set; } = -1;
        

        internal Type? ClrType;
        internal RawScannedType? RawScannedType;


        public ScannedType(string name, ScannedNamespace scannedNamespace, ScannedType? declaringType)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(scannedNamespace, nameof(scannedNamespace));
            
            Name = name;
            Namespace = scannedNamespace;

            scannedNamespace.AddType(this);

            DeclaringType = declaringType;
        }

        public virtual bool Equals(ScannedType? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name &&
                   IsClass == other.IsClass &&
                   IsInterface == other.IsInterface &&
                   IsRecord == other.IsRecord &&
                   IsPartial == other.IsPartial &&
                   Attributes.Equals(other.Attributes) &&
                   NestedTypes.Equals(other.NestedTypes) &&
                   ImplementedInterfaces.Equals(other.ImplementedInterfaces) &&
                   BaseType.NullSafeEquals(other.BaseType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
                // hashCode = (hashCode * 397) ^ NestedTypes.GetHashCode();
                // hashCode = (hashCode * 397) ^ BaseTypes.GetHashCode();
                hashCode = (hashCode * 397) ^ ImplementedInterfaces.GetHashCode();
                return hashCode;
            }
        }

        string IHaveSimpleRepresentation.ToSimpleRepresentation()
        {
            return this.FullyQualifiedName();
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            InternalPrintMembers(builder.StartAppendingFields());

            return true;
        }

        internal virtual StringBuilderExtensions.FieldStringBuilder InternalPrintMembers(
            StringBuilderExtensions.FieldStringBuilder builder)
        {
            return builder.AppendField(nameof(Name), Name)
                .AppendField(nameof(NestedTypes), NestedTypes.ToSimpleRepresentation())
                .AppendField(nameof(Attributes), Attributes)
                .AppendField(nameof(ImplementedInterfaces),
                    ImplementedInterfaces.OrderBy(i => i).ToSimpleRepresentation())
                .AppendNonDefaultField(nameof(IsClass), IsClass)
                .AppendNonDefaultField(nameof(IsInterface), IsInterface)
                .AppendNonDefaultField(nameof(IsRecord), IsRecord)
                .AppendNonDefaultField(nameof(IsPartial), IsPartial)
                .AppendNonDefaultField(nameof(DeclaringType), DeclaringType == null ? default : DeclaringType.FullyQualifiedName())
                .AppendNonDefaultField(nameof(BaseType), BaseType, t => t.NullSafeToSimpleRepresentation());
        }

        public int CompareTo(ScannedType? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

#pragma warning disable 8604
        public static bool operator <(ScannedType? left, ScannedType? right)
        {
            return Comparer<ScannedType>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(ScannedType? left, ScannedType? right)
        {
            return Comparer<ScannedType>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(ScannedType? left, ScannedType? right)
        {
            return Comparer<ScannedType>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(ScannedType? left, ScannedType? right)
        {
            return Comparer<ScannedType>.Default.Compare(left, right) >= 0;
        }
#pragma warning restore 8604
    }
}