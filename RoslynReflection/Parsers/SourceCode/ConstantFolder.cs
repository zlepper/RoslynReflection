using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynReflection.Parsers.SourceCode
{
    public class ConstantFolder
    {
        public static object? FoldExpression(ExpressionSyntax syntax)
        {
            return syntax switch
            {
                LiteralExpressionSyntax les => les.Token.Value,
                _ => throw new ArgumentOutOfRangeException(nameof(syntax), syntax.GetText(), "Unhandled expression type when constant folding")
            };
        }
    }
}