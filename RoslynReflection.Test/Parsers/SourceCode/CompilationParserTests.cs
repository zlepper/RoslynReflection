using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Parsers;
using RoslynReflection.Test.TestHelpers;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    public class CompilationParserTests
    {

        private ScannedModule GetResult(string code)
        {
            var compilation = new CompilationBuilder()
                .AddCode(code)
                .CreateCompilation();

            var result = CompilationParser.ParseCompilation(compilation);

            return result.MainModule;
        }
        
        [Test]
        public void CanLinkInheritedTypes()
        {
            var code = @"namespace MyNamespace {
    public class Parent {}
    public class Child : Parent {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("Parent")
                .Namespace
                .AddSourceClass("Child")
                .InheritFrom("MyNamespace.Parent")
                .Module;
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}