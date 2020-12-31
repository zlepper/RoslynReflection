using System;
using System.Text;

namespace RoslynReflection.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static StringBuilder AppendField(this StringBuilder builder, string fieldName, object value)
        {
            builder.Append(fieldName);
            builder.Append(" = ");
            builder.Append(value);
            return builder;
        }
    }
}