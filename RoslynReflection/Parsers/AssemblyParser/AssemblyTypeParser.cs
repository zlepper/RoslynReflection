using System;
using System.Linq;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyTypeParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _surroundingType;
        private readonly Lazy<AssemblyRecordParser> _recordParser;
        private readonly Lazy<AssemblyClassParser> _classParser;
        private readonly Lazy<AssemblyInterfaceParser> _interfaceParser;

        public AssemblyTypeParser(ScannedNamespace scannedNamespace, ScannedType? surroundingType = null)
        {
            _scannedNamespace = scannedNamespace;
            _surroundingType = surroundingType;
            _recordParser = new(() => new AssemblyRecordParser(scannedNamespace, surroundingType));
            _classParser = new(() => new AssemblyClassParser(scannedNamespace, surroundingType));
            _interfaceParser = new(() => new AssemblyInterfaceParser(scannedNamespace, surroundingType));
        }

        internal ScannedType ParseType(Type type)
        {
            var scannedType = GetConcreteType(type);

            foreach (var attribute in type.GetCustomAttributes(false))
            {
                if (RoslynReflectionConstants.HiddenNamespaces.Contains(attribute.GetType().Namespace)) continue;
                scannedType.Attributes.Add(attribute);
            }

            return scannedType;
        }

        private ScannedType GetConcreteType(Type type)
        {
            return type switch
            {
                {IsClass: true} t when t.IsRecord() => _recordParser.Value.ParseRecord(t),
                {IsClass: true} t => _classParser.Value.ParseClass(t),
                {IsInterface: true} t => _interfaceParser.Value.ParseInterface(t),
                // TODO: Bad fallback for now, until we have support for all sorts of combinations
                { } t => _classParser.Value.ParseClass(t)
            };
        }
    }

    internal static class TypeExtensions
    {
        internal static bool IsRecord(this Type type)
        {
            // "Borrowed" from https://stackoverflow.com/a/64810188/3950006
            return type.GetMethods().Any(m => m.Name == "<Clone>$");
        }
    }
}