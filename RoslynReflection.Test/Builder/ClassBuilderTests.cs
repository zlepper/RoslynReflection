using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;

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
            var _ = new ScannedClass(expectedNamespace, "MyClass");

            var actualModule = ModuleBuilder.NewBuilder()
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
            var expectedParentClass = new ScannedClass(expectedNamespace, "MyClass");
            var _ = new ScannedClass(expectedNamespace, "MyInnerClass", expectedParentClass);

            var actualModule = ModuleBuilder.NewBuilder()
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
            var expectedParentClass = new ScannedClass(expectedNamespace, "MyClass");
            var unused1 = new ScannedClass(expectedNamespace, "MyFirstInnerClass", expectedParentClass);
            var unused2 = new ScannedClass(expectedNamespace, "MySecondInnerClass", expectedParentClass);
            var unused3 = new ScannedClass(expectedNamespace, "MyInnerInnerClass", unused2);
            var unused4 = new ScannedClass(expectedNamespace, "MyThirdInnerClass", expectedParentClass);
            
            
            var actualModule = ModuleBuilder.NewBuilder()
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
            var outer = new ScannedClass(expectedNamespace, "Outer");
            var middle = new ScannedClass(expectedNamespace, "Middle", outer);
            var unused2 = new ScannedClass(expectedNamespace, "Inner", middle);

            var actual = ModuleBuilder.NewBuilder()
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
            var myClass = new ScannedClass(expectedNamespace, "MyClass");
            myClass.Attributes.Add(new SampleAttribute("hello"));

            var actual = ModuleBuilder.NewBuilder()
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
            var unused1 = new ScannedClass(ns1, "C1");
            var unused2 = new ScannedClass(ns2, "C2");

            var actualModule = ModuleBuilder.NewBuilder()
                .NewNamespace("ns1")
                .NewClass("C1")
                .NewNamespace("ns2")
                .NewClass("C2")
                .Finish();
            
            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }
    }
}