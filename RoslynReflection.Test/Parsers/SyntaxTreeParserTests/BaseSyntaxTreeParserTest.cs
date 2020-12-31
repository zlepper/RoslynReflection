using Microsoft.CodeAnalysis.CSharp;
using RoslynReflection.Models;
using RoslynReflection.Parsers.SourceCode;

namespace RoslynReflection.Test.Parsers.SyntaxTreeParserTests
{
    internal abstract class BaseSyntaxTreeParserTest
    {
        protected ScannedModule GetResult(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var module = new ScannedModule();

            var parser = new SyntaxTreeParser(module);

            parser.ParseSyntaxTree(syntaxTree);

            return module;
        }

    }
}