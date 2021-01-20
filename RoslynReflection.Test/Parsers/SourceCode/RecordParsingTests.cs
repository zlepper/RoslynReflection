using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode.Models;
using RoslynReflection.Test.TestHelpers.Extensions;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    internal class RecordParsingTests : BaseSyntaxTreeParserTest
    {
        [Test]
        public void ExtractsEmptyRecord()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public record MyRecord { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyRecord")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsMultipleRecords()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public record MyRecord { }
    public record IMyOtherInterface { }

}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyRecord")
                    .Namespace
                    .AddType("IMyOtherInterface")
                    .Module
            ));
        }
        
        [Test]
        public void ExtractsNestedRecords()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public record MyRecord {
        public record MyInnerRecord { }    
        public record MySecondInnerRecord { }    
    }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyRecord")
                    .AddNestedType("MyInnerRecord")
                    .SurroundingType!
                    .AddNestedType("MySecondInnerRecord")
                    .Module
            ));
        }
        
        /*
        [Test]
        public void HandlesPartialRecords()
        {
            //language=C#
            var code = @"namespace MyNamespace {
    public partial record MyRecord { }
    public partial record MyRecord { }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyRecord")
                    .MakePartial()
                    .Module
            ));
        }
        
        [Test]
        public void DetectsAbstractRecords()
        {
            var code = @"namespace MyNamespace {
    public abstract record MyRecord {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyRecord")
                .MakeAbstract()
                .Module));
        }*/
    }
}