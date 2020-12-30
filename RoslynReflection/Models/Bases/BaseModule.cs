using System.Collections.Generic;
using System.Linq;

namespace RoslynReflection.Models.Bases
{
    internal abstract class BaseModule : IModule
    {
        public abstract IEnumerable<INamespace> Namespaces { get; }
        public IEnumerable<IType> Types => Namespaces.SelectMany(ns => ns.Types);

        public IEnumerable<IClass> Classes => Types.OfType<IClass>();
    }
}