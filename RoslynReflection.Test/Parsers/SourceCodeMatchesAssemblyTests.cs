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
            
            AssertValues(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.True);
        }

        [Test]
        public void DetectsInterface()
        {
            var code = @"namespace MyNamespace {
    public interface IMyInterface {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.IMyInterface");
            
            AssertValues(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.False);
            AssertValues(raw.IsInterface, compiled.IsInterface, compiled.ClrType!.IsInterface, Is.True);
        }

        [Test]
        public void DetectsRecord()
        {
            var code = @"namespace MyNamespace {
    public record MyRecord {}
}";
            
            var result = AnalyzeCode(code);

            var (raw, compiled) = result.GetType("MyNamespace.MyRecord");
            
            AssertValues(raw.IsClass, compiled.IsClass, compiled.ClrType!.IsClass, Is.True);
            AssertValues(raw.IsInterface, compiled.IsInterface, compiled.ClrType!.IsInterface, Is.False);
            AssertValues(raw.IsRecord, compiled.IsRecord, compiled.ClrType!.IsRecord(), Is.True);
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
            
            AssertValues(raw.IsAbstract, compiled.IsAbstract, compiled.ClrType!.IsAbstract, Is.True);
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
            
            AssertValues(raw.IsAbstract, compiled.IsAbstract, compiled.ClrType!.IsAbstract, Is.True);
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
            AssertValues(raw.IsSealed, compiled.IsSealed, compiled.ClrType!.IsSealed, Is.True);
        }

        [Test]
        public void ParsesGenericTypes()
        {
            
            var code = @"namespace MyNamespace {
    public class MyClass<T> {}
}";

            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.MyClass");
            
            AssertValues(raw.ContainsGenericParameters, compiled.ContainsGenericParameters, compiled.ClrType!.ContainsGenericParameters, Is.True);
            AssertValues(raw.IsConstructedGenericType, compiled.IsConstructedGenericType, compiled.ClrType!.IsConstructedGenericType, Is.False);
            AssertValues(raw.IsGenericType, compiled.IsGenericType, compiled.ClrType!.IsGenericType, Is.True);
            AssertValues(raw.IsGenericTypeDefinition, compiled.IsGenericTypeDefinition, compiled.ClrType!.IsGenericTypeDefinition, Is.True);
            
            AssertMultiple(raw, compiled, type =>
            {
                Assert.That(type.Namespace.Types, Has.Exactly(1).Items);
                
                Assert.That(type.GenericTypeParameters, Has.Exactly(1).Items);
                var p = type.GenericTypeParameters[0];
                Assert.That(p.Name, Is.EqualTo("T"));
                Assert.That(ReferenceEquals(p.DeclaringType, type));
                Assert.That(p.IsGenericParameter);
                Assert.That(p.GenericParameterPosition, Is.EqualTo(0));
                
            });
        }

        [Test]
        public void ParsesSubClass()
        {
            var code = @"namespace MyNamespace {
    public class BaseClass{}
    public class SubClass : BaseClass {}
}";
            
            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.SubClass");
            
            AssertMultiple(raw, compiled, type =>
            {
                Assert.That(type.BaseType, Is.Not.Null.And.Property(nameof(ScannedType.Name)).EqualTo("BaseClass"));
                
            });
        }

        [Test]
        public void ParsesAndLinksInterface()
        {
            var code = @"namespace MyNamespace {
    public interface IInterface{}
    public class SubClass : IInterface {}
}";
            
            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.SubClass");
            
            AssertMultiple(raw, compiled, type =>
            {
                Assert.That(type.ImplementedInterfaces, Has.Exactly(1).Items);
                Assert.That(type.ImplementedInterfaces[0], Is.Not.Null.And.Property(nameof(ScannedType.Name)).EqualTo("IInterface"));
                
            });
        }

        [Test]
        public void FindsGenericBaseTypeAndSetsGenericParameters()
        {
            var code = @"namespace MyNamespace {
    public class MyOtherClass {}
    public class MyBaseClass<T> {}
    public class MySubClass : MyBaseClass<MyOtherClass> {}
}";
            
            
            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.MySubClass");

            AssertMultiple(raw, compiled, type =>
            {
                Assert.That(type.BaseType, Is.Not.Null.And.Property(nameof(ScannedType.Name)).EqualTo("MyBaseClass"));
                var bt = type.BaseType!;
                Assert.That(bt.ContainsGenericParameters, Is.False);
                Assert.That(bt.GenericTypeParameters, Is.Empty);
                Assert.That(bt.GenericTypeArguments, Has.Exactly(1).Items);

                var a = bt.GenericTypeArguments[0];
                Assert.That(a.Name, Is.EqualTo("MyOtherClass"));
                Assert.That(a.IsGenericParameter, Is.False);
            });
        }

        [Test]
        public void ConcreteGenericTypesAreTheSameObjectIfDefinitionIsTheSameEvenWhenUsedInMultiplePlaces()
        {
            var code = @"namespace MyNamespace {
    public class MyOtherClass {}
    public class MyBaseClass<T> {}
    public class MySubClass : MyBaseClass<MyOtherClass> {}
    public class MySecondSubClass : MyBaseClass<MyOtherClass> {}
}";
            
            
            var result = AnalyzeCode(code);
            
            var (rawFirst, compiledFirst) = result.GetType("MyNamespace.MySubClass");
            var (rawSecond, compiledSecond) = result.GetType("MyNamespace.MySecondSubClass");

            Assert.That(rawFirst.BaseType, Is.SameAs(rawSecond.BaseType));
            Assert.That(compiledFirst.BaseType, Is.SameAs(compiledSecond.BaseType));
        }

        [Test]
        public void GenericArgumentIsSameWhenMovedAcrossBetweenTypes()
        {
            var code = @"namespace MyNamespace {
    public class BasicBaseClass<T> {}
    public class MyBaseClass<T> : BasicBaseClass<T> {}
    public class MySubClass<T> : MyBaseClass<T> {}
}";
            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.MySubClass");
            
            AssertMultiple(raw, compiled, type =>
            {
                Assert.That(type.BaseType!.GenericTypeParameters[0], Is.SameAs(type.GenericTypeParameters[0]));
                Assert.That(type.BaseType!.BaseType!.GenericTypeParameters[0], Is.SameAs(type.GenericTypeParameters[0]));
            });
        }

        [Test]
        public void HandlesPassingGenericsToNestedGenerics()
        {
            var code = @"namespace MyNamespace {
    public class ToBeNested<T> {}
    public class MyBaseClass<T> {}
    public class MySubClass<T> : MyBaseClass<ToBeNested<T>> {}
}";
            var result = AnalyzeCode(code);
            
            var (raw, compiled) = result.GetType("MyNamespace.MySubClass");
            
            AssertMultiple(raw, compiled, type =>
            {
                var bt = type.BaseType!;
                Assert.That(bt.GenericTypeParameters, Is.Empty);
                Assert.That(bt.GenericTypeArguments, Has.Exactly(1).Items);
                var argument = bt.GenericTypeArguments[0];
                Assert.That(argument.Name, Is.EqualTo("ToBeNested"));
                Assert.That(argument.GenericTypeArguments, Is.Empty);
                Assert.That(argument.GenericTypeParameters, Has.Exactly(1).Items);
                Assert.That(type.GenericTypeParameters[0], Is.SameAs(argument.GenericTypeParameters[0]));
                
            });
        }

        [Explicit("Will always fail")]
        [Test]
        public void Expirimentation()
        {
            
            
            
            
            var subClass = typeof(MySubClass);
            var otherClass = typeof(MyOtherClass);

            Console.WriteLine(subClass);
            Console.WriteLine(otherClass);
            
            throw new Exception("You forgot to delete this!!");
        }

        private static void AssertValues<T>(T rawValue, T compiledValue, T reflectionValue, IResolveConstraint expression)
        {
            Assert.That(rawValue, expression);
            Assert.That(compiledValue, expression);
            Assert.That(reflectionValue, expression);
        }
        

        private void AssertMultiple<T>(T raw, T compiled, Action<T> doAsserts)
        {
            doAsserts(raw);
            // doAsserts(compiled);
        }
    }

    public class MyClass<T> {}

    public class MySubClass : MyClass<int> { }
    
    public class MyOtherClass : MyClass<int> {}
}