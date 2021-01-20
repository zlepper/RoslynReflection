using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynReflection.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static FieldStringBuilder AppendField(this StringBuilder builder, string fieldName, object value)
        {
            return new FieldStringBuilder(builder).AppendField(fieldName, value);
        }
        
        internal static FieldStringBuilder StartAppendingFields(this StringBuilder builder)
        {
            return new(builder);
        }

        internal struct FieldStringBuilder
        {
            private StringBuilder _stringBuilder;
            private bool _hasAddedFirstField;

            public FieldStringBuilder(StringBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;
                _hasAddedFirstField = false;
            }

            
            private void AppendFieldSeparator()
            {
                if (!_hasAddedFirstField)
                {
                    _hasAddedFirstField = true;
                    return;
                }

                _stringBuilder.Append(", ");
            }
            
            internal FieldStringBuilder AppendField(string fieldName, object? value)
            {
                AppendFieldSeparator();
                
                _stringBuilder
                    .Append(fieldName)
                    .Append(" = ")
                    .Append(value ?? "null");

                return this;
            }

            internal FieldStringBuilder AppendNonDefaultField<T>(string fieldname, T value)
            {
                if (EqualityComparer<T>.Default.Equals(value, default!)) return this;

                return AppendField(fieldname, value);
            }
            
            internal FieldStringBuilder AppendNonDefaultField<T>(string fieldname, T value, Func<T, string> format)
            {
                if (EqualityComparer<T>.Default.Equals(value, default!)) return this;

                return AppendField(fieldname, format(value));
            }
        }
    }
}