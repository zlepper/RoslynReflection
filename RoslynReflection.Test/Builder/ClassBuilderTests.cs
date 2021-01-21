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
            var _ = new ScannedType("MyClass", expectedNamespace, null)
            {
                IsClass = true
            };

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
            var _ = new ScannedType("MyInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };

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
            var unused1 = new ScannedType("MyFirstInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };
            var unused2 = new ScannedType("MySecondInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };
            var unused3 = new ScannedType("MyInnerInnerClass", expectedNamespace, unused2)
            {
                IsClass = true
            };
            var unused4 = new ScannedType("MyThirdInnerClass", expectedNamespace, expectedParentClass)
            {
                IsClass = true
            };


            var actualModule = new ScannedModule()
                            .AddNamespace("MyNamespace")
                            .AddClass("MyClass")
                            .AddNestedClass("MyFirstInnerClass")
                            .SurroundingType!
                        .AddNestedClass("MySecondInnerClass")
                        .AddNestedClass("MyInnerInnerClass")
                        .SurroundingType!
                    .SurroundingType!
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
            var middle = new ScannedType("Middle", expectedNamespace, outer)
            {
                IsClass = true
            };
            var unused2 = new ScannedType("Inner", expectedNamespace, middle)
            {
                IsClass = true
            };

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
            var unused1 = new ScannedType("C1", ns1, null)
            {
                IsClass = true,
            };
            var unused2 = new ScannedType("C2", ns2, null)
            {
                IsClass = true,
            };

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