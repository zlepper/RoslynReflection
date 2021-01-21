using System;
using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Parsers;
using RoslynReflection.Parsers.AssemblyParser;
using RoslynReflection.Test.TestHelpers;
using ScanableAssembly;

namespace RoslynReflection.Test.Parsers.Assembly
{
    [TestFixture]
    public class AssemblyParserTests
    {
        private ScannedModule ParseAssemblyFromClass<T>()
        {
            var compilation = new CompilationBuilder()
                .AddAssemblyFromType<T>()
                .CreateCompilation();

            return CompilationParser.ParseCompilation(compilation).DependsOn.Single(m => m.Name == typeof(T).Assembly.GetName().Name);
        }

        [Test]
        public void ParseAssembly()
        {
            var result = ParseAssemblyFromClass<ClassWithoutNamespace>();

            var expected = new ScannedModule(typeof(ClassWithoutNamespace).Assembly.GetName().Name!)
                .AddSingleDependency(result.DependsOn.Single())
                .AddNamespace("")
                .AddClass(nameof(ClassWithoutNamespace))
                .Module
                .AddNamespace("ScanableAssembly")
                .AddClass(nameof(ClassWithAttribute))
                .AddAttribute(new MyAttribute("Hello World"))
                .Namespace
                .AddClass(nameof(MyAttribute))
                .AddAttribute(new AttributeUsageAttribute(AttributeTargets.Class))
                // .AddBaseAssemblyClass().SetBaseType<Attribute>()
                .Namespace
                .AddClass(nameof(MySimpleClass))
                .Namespace
                .AddInterface(nameof(IMySimpleInterface))
                .Namespace
                .AddRecord(nameof(MyRecord))
                .Namespace
                .AddClass(nameof(ParentClass))
                .AddNestedClass(nameof(ParentClass.ChildClass))
                .AddNestedClass(nameof(ParentClass.ChildClass.GrandChildClass))
                .Namespace
                .AddClass(nameof(AbstractClass))
                .MakeAbstract()
                .Namespace
                .AddClass(nameof(BaseClass))
                .Namespace
                .AddClass(nameof(SubClass))
                // .AddBaseAssemblyClass().SetBaseType<BaseClass>()
                .Module; 
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
    
    
}