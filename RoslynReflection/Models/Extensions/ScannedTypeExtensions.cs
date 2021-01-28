namespace RoslynReflection.Models.Extensions
{
    public static class ScannedTypeExtensions
    {
        public static bool IsNestedType(this ScannedType type)
        {
            return type.DeclaringType != null;
        }

        public static string FullName(this ScannedType type)
        {
            if (type.DeclaringType == null)
            {
                return type.Name;
            }

            return $"{type.DeclaringType.FullName()}.{type.Name}";
        }

        public static string FullyQualifiedName(this ScannedType type)
        {
            return $"{type.Namespace.NameAsPrefix()}{type.FullName()}";
        }
    }
}