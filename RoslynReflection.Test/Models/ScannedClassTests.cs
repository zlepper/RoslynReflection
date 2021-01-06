using NUnit.Framework;
using RoslynReflection.Builder;
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
    }
}