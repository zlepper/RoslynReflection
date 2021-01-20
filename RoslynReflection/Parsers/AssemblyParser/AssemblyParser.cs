using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser.Collections;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyParser
    {
        private readonly Assembly _assembly;
        private readonly ScannedModule _module;
        private readonly NamespaceList _namespaceList;
        private readonly ScannedNamespace _rootNamespace;

        internal AssemblyParser(Assembly assembly)
        {
            _assembly = assembly;
            _module = new(assembly.GetName().Name);
            _namespaceList = new(_module);
            _rootNamespace = new(_module, "");
        }

        internal ScannedModule ParseAssembly()
        {
            foreach (var type in GetPossibleTypes(_assembly))
            {
                if (RoslynReflectionConstants.HiddenNamespaces.Contains(type.Namespace)) continue;
                AddType(type);
            }

            _module.TrimEmptyNamespaces();

            return _module;
        }

        private ScannedType AddType(Type type)
        {
            var ns = type.Namespace == null
                ? _rootNamespace
                : _namespaceList.GetNamespace(type.Namespace);

            var typeParser = new AssemblyTypeParser(ns);
            return typeParser.ParseType(type);
        }

        private static IEnumerable<Type> GetPossibleTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}