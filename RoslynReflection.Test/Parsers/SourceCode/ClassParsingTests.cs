using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Builder.Source;
using RoslynReflection.Test.Builder;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Parsers.SourceCode
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
                SourceModuleBuilder.NewBuilder()
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
                SourceModuleBuilder.NewBuilder()
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
        public class MySecondInnerClass { }    
    }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .NewInnerClass("MyInnerClass")
                    .GoBackToParent()
                    .NewInnerClass("MySecondInnerClass")
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
                SourceModuleBuilder.NewBuilder()
                    .NewNamespace("MyNamespace")
                    .NewClass("MyClass")
                    .Finish()
            ));
        }

        [Test]
        public void ExtractsUsings_InsideNamespace()
        {
            var code = @"namespace MyNamespace {
    using System;

    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .WithUsing("System")
                .Finish()));
        }
        
        [Test]
        public void ExtractsUsings_OutsideNamespace()
        {
            var code = @"using System;

namespace MyNamespace {
    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .WithUsing("System")
                .Finish()));
        }
        
        [Test]
        public void ExtractsUsingsAliases_InsideNamespace()
        {
            var code = @"namespace MyNamespace {
    using S = System;

    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .WithAliasUsing("System", "S")
                .Finish()));
        }
        
        [Test]
        public void ExtractsUsingsAliases_OutsideNamespace()
        {
            var code = @"using S = System;

namespace MyNamespace {
    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .WithAliasUsing("System", "S")
                .Finish()));
        }
        
        [Test]
        public void ExtractsUsingsSeparatelyInDifferentNamespace()
        {
            var code = @"
using Global;

namespace MyNamespace {
    using System;

    public class MyClass { }
}

namespace MyOtherNamespace {
    using RoslynReflection;

    public class MyOtherClass { }
}
";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(SourceModuleBuilder.NewBuilder()
                .NewNamespace("MyNamespace")
                .NewClass("MyClass")
                .WithUsing("Global")
                .WithUsing("System")
                .NewNamespace("MyOtherNamespace")
                .NewClass("MyOtherClass")
                .WithUsing("Global")
                .WithUsing("RoslynReflection")
                .Finish()));
        }

    }
}