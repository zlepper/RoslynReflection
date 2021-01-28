﻿using System;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.AssemblyParser
{
    internal class AssemblyTypeParser
    {
        private readonly ScannedNamespace _scannedNamespace;
        private readonly ScannedType? _declaringType;

        public AssemblyTypeParser(ScannedNamespace scannedNamespace, ScannedType? declaringType)
        {
            _scannedNamespace = scannedNamespace;
            _declaringType = declaringType;
        }

        internal ScannedType ParseType(Type type)
        {
            string name = type.Name;
            if (name.Contains("`"))
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            var scannedType = new ScannedType(name, _scannedNamespace, _declaringType)
            {
                ClrType = type
            };

            if (_declaringType != null)
            {
                _declaringType.NestedTypes.Add(scannedType);
            }
            
            ParseGenericParameters(scannedType, type);

            return scannedType;
        }

        private void ParseGenericParameters(ScannedType scannedType, Type type)
        {
            scannedType.ContainsGenericParameters = type.ContainsGenericParameters;
            scannedType.IsConstructedGenericType = type.IsConstructedGenericType;
            scannedType.IsGenericType = type.IsGenericType;
            scannedType.IsGenericTypeDefinition = type.IsGenericTypeDefinition;
        }
    }
}