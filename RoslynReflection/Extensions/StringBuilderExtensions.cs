using System.Text;

namespace RoslynReflection.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static FieldStringBuilder AppendField(this StringBuilder builder, string fieldName, object value)
        {
            return new FieldStringBuilder(builder).AppendField(fieldName, value);
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
            
            internal FieldStringBuilder AppendField(string fieldName, object value)
            {
                AppendFieldSeparator();
                
                _stringBuilder
                    .Append(fieldName)
                    .Append(" = ")
                    .Append(value);

                return this;
            }
        }
    }
}