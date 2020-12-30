using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models.FromSource;
using RoslynReflection.Parsers;

namespace RoslynReflection.Test.Parsers
{
    internal abstract class SyntaxTreeParserTest
    {
        protected SourceModule GetResult(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var module = new SourceModule();

            var parser = new SyntaxTreeParser(module);

            parser.ParseSyntaxTree(syntaxTree);

            return module;
        }

    }
}