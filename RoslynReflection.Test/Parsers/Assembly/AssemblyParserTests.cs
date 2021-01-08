using System;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser;
using ScanableAssembly;

namespace RoslynReflection.Test.Parsers.Assembly
{
    [TestFixture]
    public class AssemblyParserTests
    {
        private ScannedModule ParseAssemblyFromClass<T>()
        {
            var parser = new AssemblyParser();
            return parser.ParseAssembly(typeof(T).Assembly);
        }

        [Test]
        public void ParseAssembly()
        {
            var result = ParseAssemblyFromClass<ClassWithoutNamespace>();

            var expected = new ScannedModule()
                .AddNamespace("")
                .AddAssemblyClass<ClassWithoutNamespace>()
                .Module
                .AddNamespace("ScanableAssembly")
                .AddAssemblyClass<ClassWithAttribute>()
                .AddAttribute(new MyAttribute("Hello World"))
                .Namespace
                .AddAssemblyClass<MyAttribute>()
                .AddAttribute(new AttributeUsageAttribute(AttributeTargets.Class))
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
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
    
    
}