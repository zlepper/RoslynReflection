using System;
using System.Linq;
using RoslynReflection.Models;
using RoslynReflection.Models.Assembly;
using RoslynReflection.Models.Markers;

namespace RoslynReflection.Builder
{
    public static class AssemblyTypeBuilderExtensions
    {
        public static ScannedAssemblyClass AddNestedAssemblyClass<T>(this ScannedType type)
        {
            return new(typeof(T), type.Namespace, type);
        }
        
        
        public static ScannedAssemblyInterface AddNestedAssemblyInterface<T>(this ScannedType type)
        {
            return new(typeof(T), type.Namespace, type);
        }

        public static AssemblyBaseClassSetter<T> AddBaseAssemblyClass<T>(this T type)
        where T : class, ICanInherit
        {
            return new(type);
        }

        public readonly struct AssemblyBaseClassSetter<T>
        where T : class, ICanInherit
        {
            private readonly T _assignable;

            public AssemblyBaseClassSetter(T assignable)
            {
                _assignable = assignable;
            }

            public T SetBaseType<TBase>()
            {
                var matchedType = _assignable.Module.GetAllAvailableTypes()
                    .OfType<IScannedAssemblyType>()
                    .FirstOrDefault(t => t.Type == typeof(TBase));

                if (matchedType == null)
                {
                    throw new ArgumentException(
                        $"Cannot find scanned type matching '{typeof(TBase)}'. Is it added as a dependency?");
                }

                _assignable.ParentType = new TypeReference((ScannedType)matchedType);

                return _assignable;
            }
        }
    }
    
}