﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynReflection.Test.TestHelpers
{
    public class CompilationBuilder
    {
        private readonly List<string> _codeTrees = new();
        private readonly HashSet<Assembly> _dependentAssemblies = new();

        public CompilationBuilder AddAssemblyFromType<T>()
        {
            var assembly = typeof(T).Assembly;
            return AddAssembly(assembly);
        }

        public CompilationBuilder AddAssembly(Assembly assembly)
        {
            if (_dependentAssemblies.Contains(assembly)) return this;
            _dependentAssemblies.Add(assembly);

            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                var dependentAssembly = Assembly.Load(referencedAssembly);
                AddAssembly(dependentAssembly);
            }

            return this;
        }

        public CompilationBuilder AddCode(string code)
        {
            _codeTrees.Add(code);
            return this;
        }

        public Compilation CreateCompilation()
        {
            var syntaxTrees = _codeTrees.Select(c => CSharpSyntaxTree.ParseText(c));
            var references = _dependentAssemblies.Select(a => MetadataReference.CreateFromFile(a.Location));

            var assemblyName = Path.GetFileName(Path.GetTempFileName()).Replace(".tmp", ".dll");

            return CSharpCompilation.Create(assemblyName, syntaxTrees, references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }
    }
}