using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynReflection.Helpers;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers.SourceCode;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Parsers
{
    public class CompilationParser
    {
        private readonly Compilation _compilation;

        private CompilationParser(Compilation compilation)
        {
            _compilation = compilation;
        }

        public static ScannedModule ParseCompilation(Compilation compilation)
        {
            var parser = new CompilationParser(compilation);
            return parser.Parse();
        }

        private ScannedModule Parse()
        {
            var mainTask = Task.Run(ParseMainModule);


            var assemblyResults = ParseAssemblies().GetAwaiter().GetResult();

            var rawMainModule = mainTask.GetAwaiter().GetResult();

            var assemblyDict = assemblyResults.ToDictionary(r => r.OwnName);

            foreach (var item in assemblyDict)
            {
                foreach (var assemblyName in item.Value.DependsOn)
                {
                    var match = assemblyDict[assemblyName.Name];
                    item.Value.Module.DependsOn.Add(match.Module);
                }

                rawMainModule.DependsOn.Add(item.Value.Module);
            }

            AssemblyTypeLinker.LinkAssemblyTypes(rawMainModule.GetAllDependencies());

            var mainModule = RawScannedTypeLinker.LinkRawTypes(rawMainModule);
            
            var availableTypes = new AvailableTypes(mainModule);

            // var typeReferenceResolver = new TypeReferenceResolver(availableTypes);
            // typeReferenceResolver.ResolveUnlinkedTypes(availableTypes.Types);

            // var annotationResolver = new AnnotationResolver(availableTypes);
            // annotationResolver.ResolveAnnotations(mainModule.Types().);

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

        private RawScannedModule ParseMainModule()
        {
            var rawModule = new RawScannedModule();
            
            var syntaxTreeParser = new SyntaxTreeParser(rawModule);

            foreach (var syntaxTree in _compilation.SyntaxTrees) syntaxTreeParser.ParseSyntaxTree(syntaxTree);

            return rawModule;
        }


        private record AssemblyParseResult(ScannedModule Module, string OwnName,
            ImmutableHashSet<AssemblyName> DependsOn);
    }
}