using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;
using RoslynReflection.Parsers;
using RoslynReflection.Parsers.AssemblyParser;
using RoslynReflection.Test.TestHelpers;

namespace RoslynReflection.Test.Parsers
{
    
    [TestFixture]
    public class SourceCodeMatchesAssemblyTests
    {
        private record AnalysisResult(ScannedModule RawModule, ScannedModule CompiledModule)
        {
            internal (ScannedType Raw, ScannedType Compiled) GetType(string fullname)
            {
                var raw = RawModule.Types().Single(t => t.FullyQualifiedName() == fullname);
                var compiled = CompiledModule.Types().Single(t => t.FullyQualifiedName() == fullname);

                return (raw, compiled);
            }
        }

        private static AnalysisResult AnalyzeCode(string code)
        {
            var compilation = new CompilationBuilder()
                .AddCode(code)
                .AddAssemblyFromType<object>()
                .CreateCompilation();

            var rawCodeAnalysisResult = CompilationParser.ParseCompilation(compilation);
            
            var assemblyModule = CompileAndAnalyze(compilation);

            return new AnalysisResult(rawCodeAnalysisResult, assemblyModule);
        }

        private static ScannedModule CompileAndAnalyze(Compilation compilation)
        {
            var filename = Path.GetTempFileName().Replace(".tmp", ".dll");

            using (var outputStream = File.Open(filename, FileMode.Create, FileAccess.Write))
            {

                var r = compilation.Emit(outputStream);
                if (!r.Success)
                {
                    throw new Exception("Compilation failed: " + r.Diagnostics.First());
                }
            }

            var assembly = Assembly.LoadFrom(filename);

            var assemblyCompilation = new CompilationBuilder()
                .AddAssembly(assembly)
                .CreateCompilation();

            var assemblyAnalysisResult = CompilationParser.ParseCompilation(assemblyCompilation);

            var assemblyModule = assemblyAnalysisResult.DependsOn.Single(dep => dep.Name == assembly.GetName().Name);
            return assemblyModule;
        }
        
        [Test]
        public void DetectsClass()
        {
            var code = @"namespace MyNamespace {
    public class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyClass");
            
            TripleAssert(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.True);
        }

        [Test]
        public void DetectsInterface()
        {
            var code = @"namespace MyNamespace {
    public interface IMyInterface {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.IMyInterface");
            
            TripleAssert(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.False);
            TripleAssert(raw.IsInterface, compiled.IsInterface, compiled.ClrType!.IsInterface, Is.True);
        }

        [Test]
        public void DetectsRecord()
        {
            var code = @"namespace MyNamespace {
    public record MyRecord {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyRecord");
            
            TripleAssert(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.True);
            TripleAssert(raw.IsInterface, compiled.IsInterface, compiled.ClrType!.IsInterface, Is.False);
            TripleAssert(raw.IsRecord, compiled.IsRecord, compiled.ClrType!.IsRecord(), Is.True);
        }

        [Test]
        public void DetectsPartial()
        {
            var code = @"namespace MyNamespace {
    public partial class MyClass {}
    public partial class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, _) = result.GetType("MyNamespace.MyClass");
            
            Assert.That(raw.IsPartial, Is.True);
        }

        [Test]
        public void DetectsPartialWhenOnlyOneClass()
        {
            var code = @"namespace MyNamespace {
    public partial class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, _) = result.GetType("MyNamespace.MyClass");
            
            Assert.That(raw.IsPartial, Is.True);
        }

        [Test]
        public void DetectsAbstractClass()
        {
            var code = @"namespace MyNamespace {
    public abstract class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyClass");
            
            TripleAssert(raw.IsAbstract, compiled.IsAbstract, compiled.ClrType!.IsAbstract, Is.True);
        }

        

        [Test]
        public void DetectsAbstractPartialClass()
        {
            var code = @"namespace MyNamespace {
    public abstract partial class MyClass {}
    public partial class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyClass");
            
            TripleAssert(raw.IsAbstract, compiled.IsAbstract, compiled.ClrType!.IsAbstract, Is.True);
            Assert.That(raw.IsPartial, Is.True);
        }

        [Test]
        public void DetectsSealedClass()
        {
            var code = @"namespace MyNamespace {
    public sealed class MyClass {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyClass");
            TripleAssert(raw.IsSealed, compiled.IsSealed, compiled.ClrType!.IsSealed, Is.True);
        }

        private static void TripleAssert<T>(T value1, T value2, T value3, IResolveConstraint expression)
        {
            Assert.That(value1, expression);
            Assert.That(value2, expression);
            Assert.That(value3, expression);
        }
    }
}