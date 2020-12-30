using Microsoft.CodeAnalysis.CSharp;
using RoslynReflection.Models.FromSource;

namespace RoslynReflection.Test.Parsers.SyntaxTreeParser
{
    internal abstract class BaseSyntaxTreeParserTest
    {
        protected SourceModule GetResult(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var module = new SourceModule();

            var parser = new RoslynReflection.Parsers.SyntaxTreeParser(module);

            parser.ParseSyntaxTree(syntaxTree);

            return module;
        }

    }
}