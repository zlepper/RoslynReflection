using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RoslynReflection.Helpers
{
    internal static class AssemblyLoader
    {
        internal static async Task<IEnumerable<Assembly>> GetAssemblies(IEnumerable<AssemblyIdentity> assemblyIdentities)
        {
            var alreadyLoadedAssemblies= AppDomain.CurrentDomain.GetAssemblies().ToDictionary(a => a.GetName());

            var tasks = new List<Task<Assembly>>();
            var alreadyScannedAssemblies = new HashSet<string>();
            var allAssemblies = new HashSet<Assembly>();
            
            
            foreach (var assemblyIdentity in assemblyIdentities)
            {
                var assemblyName = new AssemblyName(assemblyIdentity.ToString());

                alreadyScannedAssemblies.Add(assemblyName.Name);

                if (alreadyLoadedAssemblies.TryGetValue(assemblyName, out var alreadyLoaded))
                {
                    tasks.Add(Task.FromResult(alreadyLoaded));
                }
                else
                {
                    tasks.Add(LoadAssembly(assemblyName));
                }
            }

            while (tasks.Count != 0)
            {
                var assemblies = await Task.WhenAll(tasks);
                tasks.Clear();
                
                foreach (var assembly in assemblies)
                {
                    allAssemblies.Add(assembly);

                    foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                    {
                        if(alreadyScannedAssemblies.Contains(referencedAssembly.Name)) continue;
                        alreadyScannedAssemblies.Add(referencedAssembly.Name);
                        
                        if (alreadyLoadedAssemblies.TryGetValue(referencedAssembly, out var alreadyLoaded))
                        {
                            tasks.Add(Task.FromResult(alreadyLoaded));
                        }
                        else
                        {
                            tasks.Add(LoadAssembly(referencedAssembly));
                        }
                    }
                }
                
            }

            return allAssemblies;
        }

        private static Task<Assembly> LoadAssembly(AssemblyName assemblyName)
        {
            return Task.Run(() => Assembly.Load(assemblyName));
        }
    }
}