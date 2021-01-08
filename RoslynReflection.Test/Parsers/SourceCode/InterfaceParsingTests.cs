using NUnit.Framework;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    internal class InterfaceParsingTests : BaseSyntaxTreeParserTest
    {
        [Test]
        public void ExtractsEmptyInterface()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public interface IMyInterface { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceClass("MyClass")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsMultipleClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public class IMyInterface { }
    public class IMyOtherInterface { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceClass("MyClass")
                    .Namespace
                    .AddSourceClass("MyOtherClass")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsNestedClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public interface IMyInterface {
        public interface IMyInnerInterface { }    
        public interface IMySecondInnerInterface { }    
    }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceClass("MyClass")
                    .AddNestedSourceClass("MyInnerClass")
                    .SurroundingType!
                    .AddNestedSourceClass("MySecondInnerClass")
                    .Module
            ));
        }
        
        [Test]
        public void HandlesPartialClasses()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public partial Interface IMyInterface { }
    public partial Interface IMyInterface { }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceClass("MyClass")
                    .Module
            ));
        }
    }
}