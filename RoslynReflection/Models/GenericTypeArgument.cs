using System;
using System.Collections.Generic;
using System.Text;
using RoslynReflection.Extensions;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public record GenericTypeArgument : IHaveSimpleRepresentation, IComparable<GenericTypeArgument>
    {
        public readonly string Name;
        
        public readonly ScannedType For;

        public GenericTypeArgument(ScannedType @for, string name)
        {
            For = @for;
            Name = name;
            @for.GenericTypeArguments.Add(this);
        }

        string IHaveSimpleRepresentation.ToSimpleRepresentation()
        {
            return Name;
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.AppendField(nameof(Name), Name);
            return true;
        }

        public virtual bool Equals(GenericTypeArgument? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public int CompareTo(GenericTypeArgument? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

#pragma warning disable 8604
        public static bool operator <(GenericTypeArgument? left, GenericTypeArgument? right)
        {
            return Comparer<GenericTypeArgument>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(GenericTypeArgument? left, GenericTypeArgument? right)
        {
            return Comparer<GenericTypeArgument>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(GenericTypeArgument? left, GenericTypeArgument? right)
        {
            return Comparer<GenericTypeArgument>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(GenericTypeArgument? left, GenericTypeArgument? right)
        {
            return Comparer<GenericTypeArgument>.Default.Compare(left, right) >= 0;
        }
#pragma warning restore 8604
    }
}