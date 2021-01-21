using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.AssemblyParser;

namespace RoslynReflection.Parsers
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
                type.Attributes.AddRange(clrType.GetCustomAttributes()
                    .Where(o => !RoslynReflectionConstants.HiddenNamespaces.Contains(o.GetType().Namespace)));
            }
        }
    }
}