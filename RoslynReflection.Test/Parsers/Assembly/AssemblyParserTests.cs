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
/*                .AddSingleDependency(result.DependsOn.Single())
                .AddNamespace("")
                .AddAssemblyClass<ClassWithoutNamespace>()
                .Module
                .AddNamespace("ScanableAssembly")
                .AddAssemblyClass<ClassWithAttribute>()
                .AddAttribute(new MyAttribute("Hello World"))
                .Namespace
                .AddAssemblyClass<MyAttribute>()
                .AddAttribute(new AttributeUsageAttribute(AttributeTargets.Class))
                .AddBaseAssemblyClass().SetBaseType<Attribute>()
                .Namespace
                .AddAssemblyClass<MySimpleClass>()
                .Namespace
                .AddAssemblyInterface<IMySimpleInterface>()
                .Namespace
                .AddAssemblyRecord<MyRecord>()
                .Namespace
                .AddAssemblyClass<ParentClass>()
                .AddNestedAssemblyClass<ParentClass.ChildClass>()
                .AddNestedAssemblyClass<ParentClass.ChildClass.GrandChildClass>()
                .Namespace
                .AddAssemblyClass<AbstractClass>()
                .MakeAbstract()
                .Namespace
                .AddAssemblyClass<BaseClass>()
                .Namespace
                .AddAssemblyClass<SubClass>()
                .AddBaseAssemblyClass().SetBaseType<BaseClass>()
                .Module*/; 
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
    
    
}