using NUnit.Framework;
using RoslynReflection.Builder;

namespace RoslynReflection.Test.Parsers.SyntaxTreeParserTests
{
    [TestFixture]
    internal class ClassParsingTests : BaseSyntaxTreeParserTest
    {
        [Test]
        public void ExtractsEmptyClass()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public class MyClass { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .Finish()
            ));
        }
        
        [Test]
        public void ExtractsMultipleClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public class MyClass { }
    public class MyOtherClass { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .NewClass("MyOtherClass")
                    .Finish()
            ));
        }
        
        [Test]
        public void ExtractsNestedClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public class MyClass {
        public class MyInnerClass { }    
    }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .NewInnerClass("MyInnerClass")
                    .Finish()
            ));
        }
        
        [Test]
        public void HandlesPartialClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public partial class MyClass { }
    public partial class MyClass { }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .Finish()
            ));
        }
    }
}