using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoslynReflection.Collections;
using RoslynReflection.Extensions;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Models
{
    public record ScannedModule : IComparable<ScannedModule>, IHaveSimpleRepresentation
    {
        public readonly string Name;
        
        public readonly ValueList<ScannedNamespace> Namespaces = new();

        public readonly ValueList<ScannedModule> DependsOn = new();

        public ScannedModule(string name = "MainCompilationModule")
        {
            Name = name;
        }

        internal void TrimEmptyNamespaces()
        {
            Namespaces.RemoveAll(ns => ns.IsEmpty());
        }

        public virtual bool Equals(ScannedModule? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Namespaces.Equals(other.Namespaces) && 
                   DependsOn.Select(dep => dep.ToSimpleRepresentation()).ToValueList().Equals(other.DependsOn.Select(dep => dep.ToSimpleRepresentation()).ToValueList());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Namespaces.GetHashCode();
                hashCode = (hashCode * 397) ^ DependsOn.Select(dep => dep.ToSimpleRepresentation()).ToValueList().GetHashCode();
                return hashCode;
            }
        }

        string IHaveSimpleRepresentation.ToSimpleRepresentation()
        {
            return Name;
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder
                .AppendField(nameof(Name), Name)
                .AppendField(nameof(Namespaces), Namespaces)
                .AppendField(nameof(DependsOn), DependsOn.OrderBy(m => m).ToSimpleRepresentation());
            return true;
        }

        public int CompareTo(ScannedModule? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator <(ScannedModule? left, ScannedModule? right)
        {
            return Comparer<ScannedModule>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(ScannedModule? left, ScannedModule? right)
        {
            return Comparer<ScannedModule>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(ScannedModule? left, ScannedModule? right)
        {
            return Comparer<ScannedModule>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(ScannedModule? left, ScannedModule? right)
        {
            return Comparer<ScannedModule>.Default.Compare(left, right) >= 0;
        }
    }
}