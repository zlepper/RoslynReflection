using RoslynReflection.Collections;

namespace RoslynReflection.Models.Markers
{
    public interface IHaveDependencies
    {
        ValueList<ScannedModule> DependsOn { get; }
    }
}