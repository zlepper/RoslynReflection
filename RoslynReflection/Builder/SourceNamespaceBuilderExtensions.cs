using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Builder
{
    public static class SourceNamespaceBuilderExtensions
    {
        public static ScannedSourceClass AddSourceClass(this ScannedNamespace ns, string name)
        {
            return new(ns, name);
        }

        public static ScannedSourceInterface AddSourceInterface(this ScannedNamespace ns, string name)
        {
            return new(ns, name);
        }

        public static ScannedSourceRecord AddSourceRecord(this ScannedNamespace ns, string name)
        {
            return new(ns, name);
        }
    }
}