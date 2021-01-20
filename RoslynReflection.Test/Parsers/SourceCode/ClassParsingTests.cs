using NUnit.Framework;
using RoslynReflection.Parsers.SourceCode.Models;
using RoslynReflection.Test.TestHelpers.Extensions;

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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyClass")
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyClass")
                    .Namespace
                    .AddType("MyOtherClass")
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyClass")
                    .AddNestedType("MyInnerClass")
                    .SurroundingType!
                    .AddNestedType("MySecondInnerClass")
                    .Module
            ));
        }
        
        /*
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
                new RawScannedModule()
                    .AddNamespace("MyNamespace")
                    .AddType("MyClass")
                    .MakePartial()
                    .Module
            ));
        }
*/
        [Test]
        public void ExtractsUsings_InsideNamespace()
        {
            var code = @"namespace MyNamespace {
    using System;

    public class MyClass { }
}";

            var result = GetResult(code);
            
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
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
            
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
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
            
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsingAlias("System", "S")
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
            
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsingAlias("System", "S")
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
            
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsing("Global")
                .AddUsing("System")
                .Module
                .AddNamespace("MyOtherNamespace")
                .AddType("MyOtherClass")
                .AddUsing("Global")
                .AddUsing("RoslynReflection")
                .Module));
        }

        /*
        [Test]
        public void DetectsAbstractClasses()
        {
            var code = @"namespace MyNamespace {
    public abstract class MyClass {}
}";

            var result = GetResult(code);
            Assert.That(result, Is.EqualTo(new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
                .MakeAbstract()
                .Module));
        }
        */

        [Test]
        public void CreatesUnlinkedInheritedType()
        {
            var code = @"namespace MyNamespace {
    public class Parent {}
    public class Child : Parent {}
}";

            var result = GetResult(code);

            var expected = new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("Parent")
                .Namespace
                .AddType("Child")
                .Module;

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ParsesGenericClass()
        {
            var code = @"namespace MyNamespace {
    public class GenericClass<T> {}
}";

            var result = GetResult(code);

            var expectedModule = new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("GenericClass")
                .Module;

            Assert.That(result, Is.EqualTo(expectedModule));
        }

        [Test]
        public void ParsesGenericExtendingGeneric()
        {
            
            var code = @"namespace MyNamespace {
    public class BaseGenericClass<T> {}
    public class ChildGenericClass<T> : BaseGenericClass<T> {}
}";


            var expectedModule = new RawScannedModule()
                .AddNamespace("MyNamespace")
                .AddType("BaseGenericClass")
                .Namespace
                .AddType("ChildGenericClass")
                .Module;
            
            var result = GetResult(code);

            Assert.That(result, Is.EqualTo(expectedModule));
        }
    }
}