using System;
using ClassWithAttribute;
using NUnit.Framework;
using RoslynReflection.Builder.Assembly;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;
using RoslynReflection.Parsers.AssemblyParser;
using SimpleClass;

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
        public void ParsesClassWithoutNamespace()
        {
            var result = ParseAssemblyFromClass<ClassWithoutNamespace>();

            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("")
                .AddAssemblyClass<ClassWithoutNamespace>()
                .Module
            ));
        }

        [Test]
        public void ParsesClassWithNamespace()
        {
            var result = ParseAssemblyFromClass<MySimpleClass>();

            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("SimpleClass")
                .AddAssemblyClass<MySimpleClass>()
                .Module
            ));
        }

        [Test]
        public void ExtractsAttributesOnClasses()
        {
            var result = ParseAssemblyFromClass<ClassWithAttribute.ClassWithAttribute>();

            var expected = new ScannedModule()
                .AddNamespace(nameof(ClassWithAttribute))
                .AddAssemblyClass<MyAttribute>()
                .AddAttribute(new AttributeUsageAttribute(AttributeTargets.Class))
                .Namespace
                .AddAssemblyClass<ClassWithAttribute.ClassWithAttribute>()
                .AddAttribute(new MyAttribute("Hello World"))
                .Module;

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}