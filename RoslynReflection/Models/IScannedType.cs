using RoslynReflection.Collections;

namespace RoslynReflection.Models
{
    public interface IScannedType
    {
        ScannedModule Module { get; }
        ScannedNamespace Namespace { get; }
        string Name { get; }
        ScannedType? SurroundingType { get; }
        ValueList<ScannedType> NestedTypes { get; }
        ValueList<object> Attributes { get; }
        ValueList<IScannedUsing> Usings { get; }
        ValueList<ScannedInterface> ImplementedInterfaces { get; }
        ValueList<GenericTypeParameter> GenericTypeArguments { get; }
    }
}