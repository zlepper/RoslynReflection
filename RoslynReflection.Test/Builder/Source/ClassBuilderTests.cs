using NUnit.Framework;
using RoslynReflection.Builder.Source;
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

            var actualModule = SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .Finish();
            
            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }

        [Test]
        public void CreatesNestedClasses()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNamespace");
            var expectedParentClass = new ScannedSourceClass(expectedNamespace, "MyClass");
            var _ = new ScannedSourceClass(expectedNamespace, "MyInnerClass", expectedParentClass);

            var actualModule = SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .NewInnerClass("MyInnerClass")
                .Finish();
            
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
            
            
            var actualModule = SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .NewInnerClass("MyFirstInnerClass")
                .GoBackToParent()
                .NewInnerClass("MySecondInnerClass")
                .NewInnerClass("MyInnerInnerClass")
                .GoBackToParent()
                .GoBackToParent()
                .NewInnerClass("MyThirdInnerClass")
                .Finish();
            
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

            var actual = SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNs")
                .NewClass("Outer")
                .NewInnerClass("Middle")
                .NewInnerClass("Inner")
                .Finish();

            Assert.That(actual, Is.EqualTo(expectedModule));
        }

        [Test]
        public void AddsAttributes()
        {
            var expectedModule = new ScannedModule();
            var expectedNamespace = new ScannedNamespace(expectedModule, "MyNs");
            var myClass = new ScannedSourceClass(expectedNamespace, "MyClass");
            myClass.Attributes.Add(new SampleAttribute("hello"));

            var actual = SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNs")
                .NewClass("MyClass")
                .WithAttribute(new SampleAttribute("hello"))
                .Finish();
            
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

            var actualModule = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns1")
                .NewClass("C1")
                .NewNamespace("ns2")
                .NewClass("C2")
                .Finish();
            
            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }
    }
}