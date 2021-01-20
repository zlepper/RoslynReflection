using Microsoft.CodeAnalysis.CSharp;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode;
using RoslynReflection.Parsers.SourceCode.Models;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    internal abstract class BaseSyntaxTreeParserTest
    {
        protected RawScannedModule GetResult(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var module = new RawScannedModule();

            var parser = new SyntaxTreeParser(module);

            parser.ParseSyntaxTree(syntaxTree);

            return module;
        }

    }
}