using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Builder
{
    public static class SourceTypeBuilderExtensions
    {
        public static T AddUsing<T>(this T type, string ns)
            where T : ScannedType
        {
            type.Usings.Add(new ScannedUsing(ns));
            return type;
        }
        
        public static T AddUsing<T>(this T type, string ns, string alias)
            where T : ScannedType
        {
            type.Usings.Add(new ScannedUsingAlias(ns, alias));
            return type;
        }
        
        public static T AddAttribute<T>(this T type, object attribute)
            where T : ScannedType
        {
            type.Attributes.Add(attribute);
            return type;
        }

        public static ScannedSourceClass AddNestedSourceClass(this ScannedType type, string name)
        {
            return new(type.Namespace, name, type);
        }
        
        public static ScannedSourceInterface AddNestedSourceInterface(this ScannedType type, string name)
        {
            return new(type.Namespace, name, type);
        }

        public static ScannedSourceRecord AddNestedSourceRecord(this ScannedType type, string name)
        {
            return new(type.Namespace, name, type);
        }
    }
}