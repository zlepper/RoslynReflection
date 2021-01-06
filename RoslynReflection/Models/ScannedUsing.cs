namespace RoslynReflection.Models
{
    public record ScannedUsing : IScannedUsing
    {
        public readonly string Namespace;

        public ScannedUsing(string ns)
        {
            Namespace = ns;
        }

        bool IScannedUsing.TryGetType(string typeName, AvailableTypes availableTypes,
            out ScannedType? type)
        {
            if (availableTypes.Namespaces.TryGetValue(Namespace, out var ns))
                foreach (var t in ns.Types)
                {
                    if (t.FullName() != typeName) continue;

                    type = t;
                    return true;
                }

            type = null;
            return false;
        }
    }
}