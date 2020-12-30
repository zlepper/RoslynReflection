using System.Collections.Generic;
using System.Linq;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers
{
    public class ParsingResult
    {
        public readonly ValueList<IModule> Modules = new();

        public IEnumerable<IType> Types => Modules.SelectMany(m => m.Types);
        public IEnumerable<IClass> Classes => Modules.SelectMany(m => m.Classes);

        public ParsingResult Combine(ParsingResult other)
        {
            var result = new ParsingResult();
            result.Modules.AddRange(Modules);
            result.Modules.AddRange(other.Modules);
            return result;
        }

        protected bool Equals(ParsingResult other)
        {
            return Modules.Equals(other.Modules);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParsingResult) obj);
        }

        public override int GetHashCode()
        {
            return Modules.GetHashCode();
        }

        public static bool operator ==(ParsingResult? left, ParsingResult? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ParsingResult? left, ParsingResult? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"ParsingResult {{ {nameof(Modules)} = {Modules}}}";
        }
    }
}