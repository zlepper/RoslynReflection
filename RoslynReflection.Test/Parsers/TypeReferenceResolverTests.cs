using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Parsers;
using RoslynReflection.Parsers.AssemblyParser;
using RoslynReflection.Test.TestHelpers;
using ScanableAssembly;

namespace RoslynReflection.Test.Parsers
{
    [TestFixture]
    public class TypeReferenceResolverTests
    {
        private ScannedModule GetResult(string code)
        {
            var compilation = new CompilationBuilder()
                .AddCode(code)
                .CreateCompilation();

            return CompilationParser.ParseCompilation(compilation);
        }
        
        [Test]
        public void CanLinkInheritedTypes()
        {
            var code = @"namespace MyNamespace {
    public class Parent {}
    public class Child : Parent {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("Parent")
                .Namespace
                .AddSourceClass("Child")
                .InheritFrom("MyNamespace.Parent")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void CanLinkImplementedTypes()
        {
            var code = @"namespace MyNamespace {
    public interface IChild {}
    public class Child : IChild {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceInterface("IChild")
                .Namespace
                .AddSourceClass("Child")
                .ImplementInterface("MyNamespace.IChild")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void CanLinkMultipleImplementedTypes()
        {
            var code = @"namespace MyNamespace {
    public interface IChild {}
    public interface IOtherChild {}
    public class Child : IChild, IOtherChild {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceInterface("IChild")
                .Namespace
                .AddSourceInterface("IOtherChild")
                .Namespace
                .AddSourceClass("Child")
                .ImplementInterface("MyNamespace.IChild")
                .ImplementInterface("MyNamespace.IOtherChild")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void SeparatesBaseClassAndInterfaces()
        {
            var code = @"namespace MyNamespace {
    public interface IChild {}
    public class Parent {}
    public class Child : Parent, IChild {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("Parent")
                .Namespace
                .AddSourceInterface("IChild")
                .Namespace
                .AddSourceClass("Child")
                .InheritFrom("MyNamespace.Parent")
                .ImplementInterface("MyNamespace.IChild")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void InterfacesImplementsInterface()
        {
            var code = @"namespace MyNamespace {
    public interface IBaseInterface {}
    public interface IChildInterface : IBaseInterface {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceInterface("IBaseInterface")
                .Namespace
                .AddSourceInterface("IChildInterface")
                .ImplementInterface("MyNamespace.IBaseInterface")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        private ScannedModule ParseAssemblyFromClass<T>()
        {
            var compilation = new CompilationBuilder()
                .AddAssemblyFromType<T>()
                .CreateCompilation();

            var assemblyName = typeof(T).Assembly.GetName().Name;
            
            return CompilationParser.ParseCompilation(compilation).DependsOn.Single(m => m.Name == assemblyName);
        }
        
        [Test]
        public void FindsClassToExtendInExternalModule()
        {
            var code = @"namespace MyNamespace {
    using ScanableAssembly;

    public class MyClass : BaseClass {}
}";

            var compilation = new CompilationBuilder()
                .AddCode(code)
                .AddAssemblyFromType<BaseClass>()
                .CreateCompilation();

            var result = CompilationParser.ParseCompilation(compilation);

            var expected = new ScannedModule()
                .AddDependency(ParseAssemblyFromClass<BaseClass>())
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("ScanableAssembly")
                .InheritFrom("ScanableAssembly.BaseClass")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}