using RoslynReflection.Models;

namespace RoslynReflection.Builder
{
    public interface IModuleBuilder
    {
        /// <summary>
        /// Creates a new namespace with the specified name
        /// </summary>
        /// <param name="name">The name of the namespace</param>
        INamespaceBuilder NewNamespace(string name);

        /// <summary>
        /// Gets the resulting module
        /// </summary>
        IModule Finish();
    }
}