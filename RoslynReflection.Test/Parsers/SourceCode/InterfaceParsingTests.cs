using NUnit.Framework;
using RoslynReflection.Builder;
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
                    .AddSourceInterface("IMyInterface")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsMultipleInterfaces()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public interface IMyInterface { }
    public interface IMyOtherInterface { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceInterface("IMyInterface")
                    .Namespace
                    .AddSourceInterface("IMyOtherInterface")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsNestedInterfaces()
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
                    .AddSourceInterface("IMyInterface")
                    .AddNestedSourceInterface("IMyInnerInterface")
                    .SurroundingType!
                    .AddNestedSourceInterface("IMySecondInnerInterface")
                    .Module
            ));
        }
        
        [Test]
        public void HandlesPartialInterfaces()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public partial interface IMyInterface { }
    public partial interface IMyInterface { }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceInterface("IMyInterface")
                    .MakePartial()
                    .Module
            ));
        }
    }
}