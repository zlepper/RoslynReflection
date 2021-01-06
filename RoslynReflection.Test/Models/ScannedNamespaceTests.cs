using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedNamespaceTests
    {
        [Test]
        public void ToStringReturnsSomething()
        {
            var ns = ModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .Finish()
                .Namespaces
                .Single();

            Assert.That(ns.ToString(), Is.Not.Empty);
        }

        [Test]
        public void Equals_ToSelfIsTrue()
        {
            var ns = ModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .Finish()
                .Namespaces
                .Single();

            Assert.That(ns.Equals(ns), Is.True);
        }
    }
}