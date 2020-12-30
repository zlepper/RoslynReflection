using System.Collections.Generic;

namespace RoslynReflection.Models
{
    public interface IModule
    {
        IEnumerable<INamespace> Namespaces { get; }
        IEnumerable<IClass> Classes { get; }
        IEnumerable<IType> Types { get; }
    }
}