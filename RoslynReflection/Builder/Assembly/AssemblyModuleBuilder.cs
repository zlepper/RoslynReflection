using RoslynReflection.Models;

namespace RoslynReflection.Builder.Assembly
{
    public class AssemblyModuleBuilder : IModuleBuilder
    {
        
        internal readonly ScannedModule Module = new();

        private AssemblyModuleBuilder()
        {
        }

        public INamespaceBuilder NewNamespace(string name)
        {
            var ns = new ScannedNamespace(Module, name);
            return new NamespaceBuilder(this, ns);
        }

        public ScannedModule Finish()
        {
            return Module;
        }

        public static IModuleBuilder NewBuilder()
        {
            return new AssemblyModuleBuilder();
        }
    }

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
        ScannedModule Finish();
    }
}