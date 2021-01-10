using System;
using JetBrains.Annotations;

namespace RoslynReflection.Helpers
{
    internal static class Guard
    {
        [ContractAnnotation("value: null => halt")]
        internal static void AgainstNull(object? value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}