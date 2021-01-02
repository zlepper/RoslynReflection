using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Parsers.AssemblyParser;
using SimpleClass;

namespace RoslynReflection.Test.Parsers.Assembly
{
    [TestFixture]
    public class AssemblyParserTests
    {
        [Test]
        public void ParsesClassWithoutNamespace()
        {
            var parser = new AssemblyParser();
            var result = parser.ParseAssembly(typeof(ClassWithoutNamespace).Assembly);

            Assert.That(result, Is.EqualTo(ModuleBuilder.NewBuilder()
                .NewNamespace("")
                .NewClass("ClassWithoutNamespace")
                .Finish()
            ));
        }
        
        [Test]
        public void ParsesClassWithNamespace()
        {
            var parser = new AssemblyParser();
            var result = parser.ParseAssembly(typeof(MySimpleClass).Assembly);

            Assert.That(result, Is.EqualTo(ModuleBuilder.NewBuilder()
                .NewNamespace("SimpleClass")
                .NewClass("MySimpleClass")
                .Finish()
            ));
        }
    }
}