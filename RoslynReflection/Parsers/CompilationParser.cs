using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;
using RoslynReflection.Parsers.SourceCode;

namespace RoslynReflection.Parsers
{
    public class CompilationParser
    {
        public static ScannedModule ParseCompilation(Compilation compilation)
        {
            var parser = new CompilationParser(compilation);
            return parser.Parse();
            
        }

        private Compilation _compilation;
        
        private CompilationParser(Compilation compilation)
        {
            _compilation = compilation;
        }

        private ScannedModule Parse()
        {
            var mainTask = Task.Run(ParseMainModule);


            var assemblyResults = ParseAssemblies().GetAwaiter().GetResult();

            var mainModule = mainTask.GetAwaiter().GetResult();

            var assemblyDict = assemblyResults.ToDictionary(r => r.OwnName);
            
            foreach (var item in assemblyDict)
            {
                foreach (var assemblyName in item.Value.DependsOn)
                {
                    var match = assemblyDict[assemblyName.Name];
                    item.Value.Module.DependsOn.Add(match.Module);
                }
                
                mainModule.DependsOn.Add(item.Value.Module);
            }

            var availableTypes = new AvailableTypes();
            availableTypes.AddNamespaces(mainModule.GetAllAvailableNamespaces());

            var typeReferenceResolver = new TypeReferenceResolver(availableTypes);
            typeReferenceResolver.ResolveUnlinkedTypes(availableTypes.Types);

            var annotationResolver = new AnnotationResolver(availableTypes);
            annotationResolver.ResolveAnnotations(mainModule.Types().OfType<IScannedSourceType>());

            return mainModule;
        }

        private AssemblyParseResult ParseAssembly(Assembly assembly)
        {
            var parser = new AssemblyParser.AssemblyParser(assembly);
            var module = parser.ParseAssembly();

            var dependsOn = assembly.GetReferencedAssemblies().ToImmutableHashSet();

            return new(module, assembly.GetName().Name, dependsOn);
        }

        private async Task<List<AssemblyParseResult>> ParseAssemblies()
        {
            var assemblies = await AssemblyLoader.GetAssemblies(_compilation.ReferencedAssemblyNames);

            var tasks = assemblies.Select(assembly => Task.Run(() => ParseAssembly(assembly)));

            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }


        private record AssemblyParseResult(ScannedModule Module, string OwnName, ImmutableHashSet<AssemblyName> DependsOn);
        
        private ScannedModule ParseMainModule()
        {
            var mainModule = new ScannedModule();

            var syntaxTreeParser = new SyntaxTreeParser(mainModule);
            
            foreach (var syntaxTree in _compilation.SyntaxTrees)
            {
                syntaxTreeParser.ParseSyntaxTree(syntaxTree);
            }

            return mainModule;
        }
    }
}