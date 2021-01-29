using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.AssemblyParser;

namespace RoslynReflection.Parsers.Linkers
{
    internal class AssemblyTypeLinker
    {
        internal static void LinkAssemblyTypes(IEnumerable<ScannedModule> modules)
        {
            var typeDict = modules
                .SelectMany(m => m.Types())
                .Where(t => t.ClrType != null)
                .ToDictionary(t => t.ClrType!);

            foreach (var type in typeDict.Values)
            {
                var clrType = type.ClrType!;

                type.IsClass = clrType.IsClass;
                type.IsInterface = clrType.IsInterface;
                type.IsPartial = false;
                type.IsRecord = clrType.IsRecord();
                type.IsAbstract = clrType.IsAbstract;
                type.IsSealed = clrType.IsSealed;
                type.Attributes.AddRange(clrType.GetCustomAttributes()
                    .Where(o => !RoslynReflectionConstants.HiddenNamespaces.Contains(o.GetType().Namespace)));

                ParseGenericParameters(type, clrType);
            }
        }

        private static void ParseGenericParameters(ScannedType scannedType, Type type)
        {
            scannedType.ContainsGenericParameters = type.ContainsGenericParameters;
            scannedType.IsConstructedGenericType = type.IsConstructedGenericType;
            scannedType.IsGenericType = type.IsGenericType;
            scannedType.IsGenericTypeDefinition = type.IsGenericTypeDefinition;

            if (type.IsGenericType)
            {
                foreach (var genericArgument in type.GetGenericArguments())
                {
                    var scannedGenericParameter = new ScannedType(genericArgument.Name, scannedType.Namespace, scannedType);
                    scannedType.GenericTypeParameters.Add(scannedGenericParameter);
                    scannedGenericParameter.IsGenericParameter = true;
                    scannedGenericParameter.GenericParameterPosition = genericArgument.GenericParameterPosition;
                }
            }
        }
    }
}