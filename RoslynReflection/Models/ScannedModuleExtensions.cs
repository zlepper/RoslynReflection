﻿using System.Collections.Generic;
using System.Linq;

namespace RoslynReflection.Models
{
    public static class ScannedModuleExtensions
    {
        public static IEnumerable<ScannedType> GetAllAvailableTypes(this ScannedModule module)
        {
            return module.GetAllDependencies()
                .SelectMany(m => m.Types())
                .Concat(module.Types());
        }

        public static IEnumerable<ScannedNamespace> GetAllAvailableNamespaces(this ScannedModule module)
        {
            return module.GetAllDependencies()
                .SelectMany(m => m.Namespaces)
                .Concat(module.Namespaces);
        }

        public static IEnumerable<ScannedModule> GetAllDependencies(this ScannedModule module)
        {
            var modules = new HashSet<ScannedModule>();
            var queue = new Queue<ScannedModule>();
            queue.Enqueue(module);

            while (queue.Count != 0)
            {
                var next = queue.Dequeue();

                foreach (var dep in next.DependsOn)
                {
                    if (!modules.Contains(dep))
                    {
                        modules.Add(dep);
                        queue.Enqueue(dep);
                    }
                }
            }


            return modules;
        }
    }
}