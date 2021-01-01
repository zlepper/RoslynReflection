using System.Collections.Generic;
using System.Linq;

namespace RoslynReflection.Models
{
    public static class ModuleExtensions
    {
        public static IEnumerable<ScannedType> Types(this ScannedModule module)
        {
            return module.Namespaces.SelectMany(ns => ns.Types);
        }

        public static IEnumerable<ScannedClass> Classes(this ScannedModule module)
        {
            return module.Types().OfType<ScannedClass>();
        }
    }
}