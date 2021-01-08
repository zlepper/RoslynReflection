using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using RoslynReflection.Parsers.SourceCode;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynReflection.Test.Parsers.SourceCode
{
    [TestFixture]
    public class ConstantFolderTest
    {
        [Test]
        public void ParsesString()
        {
            var input = LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("Hello"));

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo("Hello"));
        }
        
        [Test]
        public void ParsesInt()
        {
            var input = LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(42));

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo(42));
        }
        
        [Test]
        public void ParsesDouble()
        {
            var input = LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(42.5));

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo(42.5));
        }
        
        [Test]
        public void ParsesBool_True()
        {
            var input = LiteralExpression(SyntaxKind.TrueLiteralExpression);

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo(true));
        }
        
        [Test]
        public void ParsesBool_False()
        {
            var input = LiteralExpression(SyntaxKind.FalseLiteralExpression);

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo(false));
        }
        
        [Test]
        public void ParsesNull()
        {
            var input = LiteralExpression(SyntaxKind.NullLiteralExpression);

            var result = ConstantFolder.FoldExpression(input);
            
            Assert.That(result, Is.EqualTo(null));
        }
    }
}