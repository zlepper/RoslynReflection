using System.Runtime.CompilerServices;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Builder
{
    [TestFixture]
    public class ClassBuilderTests
    {
        [Test]
        public void CreatesClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var type = new ScannedType("MyClass", expectedNamespace, null)
            {
                IsClass = true
            };
            expectedNamespace.AddType(type);

            var actualModule = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddClass("MyClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void CreatesNestedClasses()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var expectedParentClass = new ScannedType("MyClass", expectedNamespace, null)
            {
                IsClass = true
            };
            expectedNamespace.AddType(expectedParentClass);
            var innerClass = new ScannedType("MyInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };
            expectedParentClass.NestedTypes.Add(innerClass);
            expectedNamespace.AddType(innerClass);

            var actualModule = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddClass("MyClass")
                .AddNestedClass("MyInnerClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void AllowsNavigatingBackUpToCreateNewPropertiesOnParentClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var expectedParentClass = new ScannedType("MyClass", expectedNamespace, null)
            {
                IsClass = true
            };
            expectedNamespace.AddType(expectedParentClass);
            
            var firstInnerClass = new ScannedType("MyFirstInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true,
            };
            expectedParentClass.NestedTypes.Add(firstInnerClass);
            expectedNamespace.AddType(firstInnerClass);
            
            var secondInnerClass = new ScannedType("MySecondInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };
            expectedParentClass.NestedTypes.Add(secondInnerClass);
            expectedNamespace.AddType(secondInnerClass);
            
            var nestedInnerInnerClass = new ScannedType("MyInnerInnerClass", expectedNamespace, secondInnerClass)
            {
                IsClass = true
            };
            secondInnerClass.NestedTypes.Add(nestedInnerInnerClass);
            expectedNamespace.AddType(nestedInnerInnerClass);
            
            var thirdInnerClass = new ScannedType("MyThirdInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };
            expectedParentClass.NestedTypes.Add(thirdInnerClass);
            expectedNamespace.AddType(thirdInnerClass);

            
            var actualModule = new ScannedModule()
                            .AddNamespace("MyNamespace")
                            .AddClass("MyClass")
                            .AddNestedClass("MyFirstInnerClass")
                            .DeclaringType!
                        .AddNestedClass("MySecondInnerClass")
                        .AddNestedClass("MyInnerInnerClass")
                        .DeclaringType!
                    .DeclaringType!
                .AddNestedClass("MyThirdInnerClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void DoubleNestedClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNs");
            var outer = new ScannedType("Outer", expectedNamespace, null)
            {
                IsClass = true,
            };
            expectedNamespace.AddType(outer);
            
            var middle = new ScannedType("Middle", expectedNamespace, outer)
            {
                IsClass = true
            };
            outer.NestedTypes.Add(middle);
            expectedNamespace.AddType(middle);
            
            var inner = new ScannedType("Inner", expectedNamespace, middle)
            {
                IsClass = true
            };
            middle.NestedTypes.Add(inner);
            expectedNamespace.AddType(inner);

            var actual = new ScannedModule()
                .AddNamespace("MyNs")
                .AddClass("Outer")
                .AddNestedClass("Middle")
                .AddNestedClass("Inner")
                .Module;

            Assert.That(actual, Is.EqualTo(expectedModule));
        }

        [Test]
        public void AddsAttributes()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNs");
            var _ = new ScannedType("MyClass", expectedNamespace, null)
            {
                IsClass = true,
                Attributes = {new SampleAttribute("hello")}
            };

            var actual = new ScannedModule()
                .AddNamespace("MyNs")
                .AddClass("MyClass")
                .AddAttribute(new SampleAttribute("hello"))
                .Module;

            Assert.That(actual, Is.EqualTo(actual));
        }

        [Test]
        public void AddsNewNamespaceFromClass()
        {
            var expectedModule = new ScannedModule();
            var ns1 = new ScannedNamespace(expectedModule, "ns1");
            var ns2 = new ScannedNamespace(expectedModule, "ns2");
            var c1 = new ScannedType("C1", ns1, null)
            {
                IsClass = true,
            };
            ns1.AddType(c1);
            
            var c2 = new ScannedType("C2", ns2, null)
            {
                IsClass = true,
            };
            ns2.AddType(c2);

            var actualModule = new ScannedModule()
                .AddNamespace("ns1")
                .AddClass("C1")
                .Module
                .AddNamespace("ns2")
                .AddClass("C2")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }
    }
}