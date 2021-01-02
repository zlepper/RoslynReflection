namespace RoslynReflection.Models
{
    public static class TypeExtensions
    {
        public static bool IsNestedType(this ScannedType type)
        {
            return type.SurroundingType != null;
        }

        public static string FullName(this ScannedType type)
        {
            if (type.SurroundingType == null)
            {
                return type.Name;
            }

            return $"{type.SurroundingType.FullName()}.{type.Name}";
        }

        public static string FullyQualifiedName(this ScannedType type)
        {
            return $"{type.Namespace.NameAsPrefix()}{type.FullName()}";
        }
    }
}