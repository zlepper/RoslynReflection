using System.Collections.Generic;

namespace RoslynReflection.Models
{
    public interface INamespace
    {
        IModule Module { get; }
        IEnumerable<IType> Types { get; }
        IEnumerable<IClass> Classes { get; }
        
        string Name { get; }
    }
}