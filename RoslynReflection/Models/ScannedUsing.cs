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
                return ns.TryGetType(typeName, out type);

            type = null;
            return false;
        }
    }
}