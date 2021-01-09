namespace RoslynReflection.Models
{
    /// <summary>
    /// A special namespace that doesn't actually save the items added to it
    /// </summary>
    internal record HiddenNamespace : ScannedNamespace
    {
        internal static readonly HiddenNamespace Instance = new(new ScannedModule(), "");
        
        private HiddenNamespace(ScannedModule module, string name) : base(module, name)
        {
        }

        internal override void AddType(ScannedType type)
        {
            // Intentionally empty
        }
        
        
    }
}