using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedTypeTests
    {
        [Test]
        public void ModuleRefersToModuleOfClass()
        {
            var module = SourceModuleBuilder.NewBuilder()
                .NewNamespace("ns")
                .NewClass("MyClass")
                .Finish();

            var klass = module.Types().Single(t => t.Name == "MyClass");
            
            Assert.That(ReferenceEquals(klass.Module, module));
        }
    }
}