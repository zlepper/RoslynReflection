namespace RoslynReflection.Models
{
    public static class TypeExtensions
    {
        public static bool IsNestedType(this IType type)
        {
            return type.SurroundingType != null;
        }

        public static string FullName(this IType type)
        {
            if (type.SurroundingType == null)
            {
                return type.Name;
            }

            return $"{type.SurroundingType.FullName()}.{type.Name}";
        }

        public static string FullyQualifiedName(this IType type)
        {
            return $"{type.Namespace.Name}.{type.FullName()}";
        }
    }
}