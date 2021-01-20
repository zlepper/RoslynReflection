namespace RoslynReflection.Models
{
    public static class TypeExtensions
    {
        public static bool IsNestedType(this IScannedType type)
        {
            return type.SurroundingType != null;
        }

        public static string FullName(this IScannedType type)
        {
            if (type.SurroundingType == null)
            {
                return type.Name;
            }

            return $"{type.SurroundingType.FullName()}.{type.Name}";
        }

        public static string FullyQualifiedName(this IScannedType type)
        {
            return $"{type.Namespace.NameAsPrefix()}{type.FullName()}";
        }
    }
}