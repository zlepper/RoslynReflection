using System.Linq;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers
{
    internal class RawScannedTypeLinker
    {
        internal static ScannedModule LinkRawTypes(RawScannedModule module)
        {
            return GetUnlinkedScannedModule(module);
        }

        internal static ScannedModule GetUnlinkedScannedModule(RawScannedModule rawModule)
        {
            var scannedModule = new ScannedModule();

            foreach (var rawNamespace in rawModule.Namespaces)
            {
                var scannedNamespace = new ScannedNamespace(scannedModule, rawNamespace.Name);

                foreach (var rawType in rawNamespace.Types.Where(t => t.SurroundingType == null))
                {
                    CreateScannedType(scannedNamespace, rawType, null);
                }
            }

            return scannedModule;
        }

        private static ScannedType CreateScannedType(ScannedNamespace ns, RawScannedType rawType, ScannedType? surroundingType)
        {
            var type = new ScannedType(rawType.Name, ns, surroundingType)
            {
                RawScannedType = rawType
            };

            foreach (var rawNestedType in rawType.NestedTypes)
            {
                CreateScannedType(ns, rawNestedType, type);
            }

            return type;
        }
    }
}