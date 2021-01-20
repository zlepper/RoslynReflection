using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Parsers.SourceCode.Models;
using RoslynReflection.Test.TestHelpers.Extensions;

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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("IMyInterface")
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("IMyInterface")
                    .Namespace
                    .AddType("IMyOtherInterface")
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("IMyInterface")
                    .AddNestedType("IMyInnerInterface")
                    .SurroundingType!
                    .AddNestedType("IMySecondInnerInterface")
                    .Module
            ));
        }
        
        /*
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("IMyInterface")
                    .MakePartial()
                    .Module
            ));
        }*/
    }
}