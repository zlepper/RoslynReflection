using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Test.Builder;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedClassTests
    {
        [Test]
        public void ComparesAttributes()
        {
            var left = ModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .WithAttribute(new SampleAttribute("hello"))
                .Finish();

            var right = ModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .WithAttribute(new SampleAttribute("world"))
                .Finish();
            
            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void ToStringReturnsSomething()
        {
            var klass = ModuleBuilder.NewBuilder()
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
            var klass = ModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .Finish()
                .Classes()
                .Single();
            
            Assert.That(klass.Equals(klass));
        }
    }
}