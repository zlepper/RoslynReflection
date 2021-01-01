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
            
            
            var actualModule = ModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .NewInnerClass("MyFirstInnerClass")
                .GoBackToParent()
                .NewInnerClass("MySecondInnerClass")
                .Finish();
            
            Assert.That(actualModule, Is.EqualTo(expectedModule));
        }
    }
}