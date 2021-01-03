using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RoslynReflection.Collections;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyParser
    {
        private ScannedModule _module = new();
        private NamespaceList _namespaceList;
        private Dictionary<Type, ScannedType> _typeDict = new();
        private ScannedNamespace _rootNamespace;
        
        internal AssemblyParser()
        {
            _namespaceList = new(_module);
            _rootNamespace = new(_module, "");
        }
        
        internal ScannedModule ParseAssembly(Assembly assembly)
        {
            foreach (var type in GetPossibleTypes(assembly))
            {
                if (RoslynReflectionConstants.HiddenNamespaces.Contains(type.Namespace)) continue;
                AddType(type);
            }
            
            _module.TrimEmptyNamespaces();

            return _module;
        }

        private ScannedType AddType(Type type)
        {
            if(_typeDict.TryGetValue(type, out var existing)) return existing;
                
            var ns = type.Namespace == null 
                ? _rootNamespace 
                : _namespaceList.GetNamespace(type.Namespace);

            var surroundingType = type.DeclaringType == null ? null : AddType(type.DeclaringType);

            return _typeDict[type] = new ScannedClass(ns, type.Name, surroundingType);
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