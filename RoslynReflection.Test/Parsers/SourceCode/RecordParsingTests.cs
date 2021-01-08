using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;

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
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceRecord("MyRecord")
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
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceRecord("MyRecord")
                    .Namespace
                    .AddSourceRecord("IMyOtherInterface")
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
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceRecord("MyRecord")
                    .AddNestedSourceRecord("MyInnerRecord")
                    .SurroundingType!
                    .AddNestedSourceRecord("MySecondInnerRecord")
                    .Module
            ));
        }
        
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
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceRecord("MyRecord")
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
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceRecord("MyRecord")
                .MakeAbstract()
                .Module));
        }
    }
}