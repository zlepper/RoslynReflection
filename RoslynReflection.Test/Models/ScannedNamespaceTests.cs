using NUnit.Framework;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedNamespaceTests
    {
        [Test]
        public void ToStringReturnsSomething()
        {
            var ns = new ScannedModule()
                .AddNamespace("ns");

            Assert.That(ns.ToString(), Is.Not.Empty);
        }

        [Test]
        public void Equals_ToSelfIsTrue()
        {
            var ns = new ScannedModule()
                .AddNamespace("ns");

            Assert.That(ns.Equals(ns), Is.True);
        }
    }
}