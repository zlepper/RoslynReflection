using Microsoft.CodeAnalysis;
using NUnit.Framework;
using RoslynReflection.Builder;

namespace RoslynReflection.Test.Parsers
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    internal class SyntaxTreeParser_NamespaceParsing : SyntaxTreeParserTest
    {
        
        [Test]
        public void ExtractsEmptyNamespace()
        {
            //language=C#
            var code = @"namespace MyNamespace {}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .Finish()
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
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewNamespace("MySecondNamespace")
                    .Finish()
            ));
        }

        [Test]
        public void ExtractsNestedNamespaces()
        {
            //language=C#
            var code = @"namespace MyNamespace { namespace MyInnerNamespace { } }";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                ModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewNamespace("MyNamespace.MyInnerNamespace")
                    .Finish()
            ));
        }
    }
}