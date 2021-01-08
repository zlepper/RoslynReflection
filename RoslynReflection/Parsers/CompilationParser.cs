using System;
using System.Collections.Generic;
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
        
        
        public static CompilationAnalysisResult ParseCompilation(Compilation compilation)
        {
            var parser = new CompilationParser(compilation);
            return parser.Parse();
            
        }

        private Compilation _compilation;
        
        private CompilationParser(Compilation compilation)
        {
            _compilation = compilation;
        }

        private CompilationAnalysisResult Parse()
        {
            var mainTask = Task.Run(ParseMainModule);

            var assemblyTasks = _compilation.ReferencedAssemblyNames
                .Select(reference => Task.Run(() => ParseAssembly(reference)))
                .ToList();


            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(assemblyTasks.Concat(new []{mainTask}).ToArray());

            var mainModule = mainTask.Result;

            var result = new CompilationAnalysisResult(mainModule);
            result.Dependencies.AddRange(assemblyTasks.Select(t => t.Result));

            var availableTypes = new AvailableTypes();
            availableTypes.AddNamespaces(result.Dependencies.SelectMany(m => m.Namespaces));
            availableTypes.AddNamespaces(mainModule.Namespaces);

            var annotationResolver = new AnnotationResolver(availableTypes);
            annotationResolver.ResolveAnnotations(mainModule.Types().OfType<IScannedSourceType>());

            return result;
        }

        private ScannedModule ParseAssembly(AssemblyIdentity assemblyIdentity)
        {
            var assembly = Assembly.Load(assemblyIdentity.ToString());
            var parser = new AssemblyParser.AssemblyParser();
            return parser.ParseAssembly(assembly);
        }
        
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