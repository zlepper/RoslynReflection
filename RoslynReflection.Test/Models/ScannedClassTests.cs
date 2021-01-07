using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;
using RoslynReflection.Test.Builder;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedClassTests
    {
        [Test]
        public void ComparesAttributes()
        {
            var left = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .WithAttribute(new SampleAttribute("hello"))
                .Finish();

            var right = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .WithAttribute(new SampleAttribute("world"))
                .Finish();
            
            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void ToStringReturnsSomething()
        {
            var klass = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .Finish()
                .Classes()
                .Single();
            
            Assert.That(klass.ToString(), Is.Not.Empty);
        }

        [Test]
        public void Equals_ReturnsTrueIfReferenceAreTheSame()
        {
            var klass = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .Finish()
                .Classes()
                .Single();
            
            Assert.That(klass.Equals(klass));
        }
    }
}