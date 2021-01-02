namespace RoslynReflection.Models
{
    public static class NamespaceExtensions
    {
        internal static string NameAsPrefix(this ScannedNamespace ns)
        {
            if (ns.Name == "")
            {
                return "";
            }

            return ns.Name + ".";
        }
    }
}