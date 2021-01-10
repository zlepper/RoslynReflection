using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

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
    public class MyClass { }
    public class MyOtherClass { }

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
    public class MyClass {
        public class MyInnerClass { }    
        public class MySecondInnerClass { }    
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
    public partial class MyClass { }
    public partial class MyClass { }
}";

            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(
                new ScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddSourceClass("MyClass")
                    .MakePartial()
                    .Module
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
            
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("System")
                .Module));
        }
        
        [Test]
        public void ExtractsUsings_OutsideNamespace()
        {
            var code = @"using System;

namespace MyNamespace {
    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("System")
                .Module));
        }
        
        [Test]
        public void ExtractsUsingsAliases_InsideNamespace()
        {
            var code = @"namespace MyNamespace {
    using S = System;

    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("System", "S")
                .Module));
        }
        
        [Test]
        public void ExtractsUsingsAliases_OutsideNamespace()
        {
            var code = @"using S = System;

namespace MyNamespace {
    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("System", "S")
                .Module));
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
            
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .AddUsing("Global")
                .AddUsing("System")
                .Module
                .AddNamespace("MyOtherNamespace")
                .AddSourceClass("MyOtherClass")
                .AddUsing("Global")
                .AddUsing("RoslynReflection")
                .Module));
        }

        [Test]
        public void DetectsAbstractClasses()
        {
            var code = @"namespace MyNamespace {
    public abstract class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("MyClass")
                .MakeAbstract()
                .Module));
        }

        [Test]
        public void CreatesUnlinkedInheritedType()
        {
            var code = @"namespace MyNamespace {
    public class Parent {}
    public class Child : Parent {}
}";

            var result = GetResult(code);

            var expected = new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddSourceClass("Parent")
                .Namespace
                .AddSourceClass("Child");

            expected.BaseTypes.Add(new UnlinkedType("Parent"));
            
            Assert.That(result, Is.EqualTo(expected.Module));
        }

        [Test]
        public void ParsesGenericClass()
        {
            var code = @"namespace MyNamespace {
    public class GenericClass<T> {}
}";

            var result = GetResult(code);

            var expectedModule = new ScannedModule();
            var ns = expectedModule.AddNamespace("MyNamespace");
            var expectedClass = ns.AddSourceClass("GenericClass");
            var _ = new GenericTypeArgument(expectedClass, "T");

            Assert.That(result, Is.EqualTo(expectedModule));

        }
    }
}