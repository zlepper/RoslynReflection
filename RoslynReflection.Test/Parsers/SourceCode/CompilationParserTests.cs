using NUnit.Framework;
using RoslynReflection.Test.TestHelpers;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    public class CompilationParserTests
    {
        public void ParsesSyntaxTreesFromCompilation()
        {
            var compilation = new CompilationBuilder()
                .AddCode(@"namespace MyNamespace { public class MyClass { } }")
                .CreateCompilation();
            
            
        } 
    }
}