using NUnit.Framework;
using RoslynReflection.Parsers.SourceCode.Models;
using RoslynReflection.Test.TestHelpers.Extensions;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    internal class NamespaceParsingTests : BaseSyntaxTreeParserTest
    {
        
        [Test]
        public void ExtractsEmptyNamespace()
        {
            //language=C#
            var code = @"namespace MyNamespace {}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .Module
            ));
        }

        [Test]
        public void ExtractsMultipleNamespaces()
        {
            //language=C#
            var code = @"namespace MyNamespace {} 
namespace MySecondNamespace";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .Module
                    .AddNamespace("MySecondNamespace")
                    .Module
            ));
        }

        [Test]
        public void ExtractsNestedNamespaces()
        {
            //language=C#
            var code = @"namespace MyNamespace { namespace MyInnerNamespace { } }";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .Module
                    .AddNamespace("MyNamespace.MyInnerNamespace")
                    .Module
            ));
        }
    }
}