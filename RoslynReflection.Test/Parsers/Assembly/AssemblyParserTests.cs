using System;
using System.Collections.Generic;
using System.Linq;
using ClassWithAttribute;
using NUnit.Framework;
using RoslynReflection.Builder;
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

            Assert.That(result, Is.EqualTo(ModuleBuilder.NewBuilder()
                .NewNamespace("")
                .NewClass("ClassWithoutNamespace")
                .Finish()
            ));
        }
        
        [Test]
        public void ParsesClassWithNamespace()
        {
            var result = ParseAssemblyFromClass<MySimpleClass>();

            Assert.That(result, Is.EqualTo(ModuleBuilder.NewBuilder()
                .NewNamespace("SimpleClass")
                .NewClass("MySimpleClass")
                .Finish()
            ));
        }

        [Test]
        public void ExtractsAttributesOnClasses()
        {
            var result = ParseAssemblyFromClass<ClassWithAttribute.ClassWithAttribute>();

            var expected = ModuleBuilder.NewBuilder()
                .NewNamespace(nameof(ClassWithAttribute))
                .NewClass(nameof(MyAttribute))
                .WithAttribute(new AttributeUsageAttribute(AttributeTargets.Class))
                .NewClass(nameof(ClassWithAttribute.ClassWithAttribute))
                .WithAttribute(new MyAttribute("Hello World"))
                .Finish();

            var eq = EqualityComparer<ScannedNamespace>.Default.Equals(result.Namespaces.First(),
                expected.Namespaces.First());
            var eq2 = result.Namespaces.GetHashCode() == expected.Namespaces.GetHashCode();
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}