using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynReflection.Models;

namespace RoslynReflection.Parsers.SourceCode
{
    internal class UsingDeclarationParser
    {
        internal static IEnumerable<IScannedUsing> ExtractUsings(SyntaxNode node)
        {
            return node.ChildNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select<UsingDirectiveSyntax, IScannedUsing>(decl =>
                {
                    var import = decl.Name.GetText().ToString().Trim();
                    if (decl.Alias != null)
                    {
                        return new ScannedUsingAlias(import, decl.Alias.Name.GetText().ToString().Trim());
                    }

                    return new ScannedUsing(import);
                });
        }
    }
}