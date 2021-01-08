using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Builder.Source
{
    [TestFixture]
    public class ClassBuilderTests
    {
        [Test]
        public void CreatesClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var _ = new ScannedSourceClass(expectedNamespace, "MyClass");

            var actualModule = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void CreatesNestedClasses()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var expectedParentClass = new ScannedSourceClass(expectedNamespace, "MyClass");
            var _ = new ScannedSourceClass(expectedNamespace, "MyInnerClass", expectedParentClass);

            var actualModule = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddNestedSourceClass("MyInnerClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void AllowsNavigatingBackUpToCreateNewPropertiesOnParentClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var expectedParentClass = new ScannedSourceClass(expectedNamespace, "MyClass");
            var unused1 = new ScannedSourceClass(expectedNamespace, "MyFirstInnerClass", expectedParentClass);
            var unused2 = new ScannedSourceClass(expectedNamespace, "MySecondInnerClass", expectedParentClass);
            var unused3 = new ScannedSourceClass(expectedNamespace, "MyInnerInnerClass", unused2);
            var unused4 = new ScannedSourceClass(expectedNamespace, "MyThirdInnerClass", expectedParentClass);


            var actualModule = new ScannedModule()
                            .AddNamespace("MyNamespace")
                            .AddSourceClass("MyClass")
                            .AddNestedSourceClass("MyFirstInnerClass")
                            .SurroundingType!
                        .AddNestedSourceClass("MySecondInnerClass")
                        .AddNestedSourceClass("MyInnerInnerClass")
                        .SurroundingType!
                    .SurroundingType!
                .AddNestedSourceClass("MyThirdInnerClass")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void DoubleNestedClass()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNs");
            var outer = new ScannedSourceClass(expectedNamespace, "Outer");
            var middle = new ScannedSourceClass(expectedNamespace, "Middle", outer);
            var unused2 = new ScannedSourceClass(expectedNamespace, "Inner", middle);

            var actual = new ScannedModule()
                .AddNamespace("MyNs")
                .AddSourceClass("Outer")
                .AddNestedSourceClass("Middle")
                .AddNestedSourceClass("Inner")
                .Module;

            Assert.That(actual, Is.EqualTo(expectedModule));
        }

        [Test]
        public void AddsAttributes()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNs");
            var myClass = new ScannedSourceClass(expectedNamespace, "MyClass");
            myClass.Attributes.Add(new SampleAttribute("hello"));

            var actual = new ScannedModule()
                .AddNamespace("MyNs")
                .AddSourceClass("MyClass")
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
            var unused1 = new ScannedSourceClass(ns1, "C1");
            var unused2 = new ScannedSourceClass(ns2, "C2");

            var actualModule = new ScannedModule()
                .AddNamespace("ns1")
                .AddSourceClass("C1")
                .Module
                .AddNamespace("ns2")
                .AddSourceClass("C2")
                .Module;

            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }
    }
}