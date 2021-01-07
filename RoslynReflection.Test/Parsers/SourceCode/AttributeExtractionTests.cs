using NUnit.Framework;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;
using RoslynReflection.Parsers;
using RoslynReflection.Test.TestHelpers;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    internal class AttributeExtractionTests
    {

        private ScannedModule GetResult(string code)
        {
            var compilation = new CompilationBuilder()
                .AddCode(code)
                .AddAssemblyFromType<AttributeExtractionTests>()
                .CreateCompilation();

            var result = CompilationParser.ParseCompilation(compilation);

            return result.MainModule;
        }
        
        [Test]
        public void ParsesExternalAnnotations()
        {
            var code = @"
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace MyNamespace {
    [Sample(""Hello"")]
    [Another(""World"")]
    public class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .WithUsing("RoslynReflection.Test.TestHelpers.TestAttributes")
                    .WithAttribute(new SampleAttribute("Hello"))
                    .WithAttribute(new AnotherAttribute("World"))
                    .Finish()
            ));
        }
        
        [Test]
        public void ParsesExternalAnnotations_AsInlineList()
        {
            var code = @"
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace MyNamespace {
    [Sample(""Hello""), Another(""World"")]
    public class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .WithUsing("RoslynReflection.Test.TestHelpers.TestAttributes")
                    .WithAttribute(new SampleAttribute("Hello"))
                    .WithAttribute(new AnotherAttribute("World"))
                    .Finish()
            ));
        }
        
        [Test]
        public void InvokesEmptyAttributeConstructor_NoParentheses()
        {
            var code = @"
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace MyNamespace {
    [Empty]
    public class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .WithUsing("RoslynReflection.Test.TestHelpers.TestAttributes")
                    .WithAttribute(new EmptyAttribute())
                    .Finish()
            ));
        }
        
        [Test]
        public void InvokesEmptyAttributeConstructor_EmptyParentheses()
        {
            var code = @"
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace MyNamespace {
    [Empty()]
    public class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .WithUsing("RoslynReflection.Test.TestHelpers.TestAttributes")
                    .WithAttribute(new EmptyAttribute())
                    .Finish()
            ));
        }
    }
}