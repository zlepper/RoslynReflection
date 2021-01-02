using System.Collections.Generic;

namespace RoslynReflection
{
    public class RoslynReflectionConstants
    {
        /// <summary>
        /// Namespaces that is usually added by some instrumentation tool or equivalent.
        ///
        /// Feel free to add your own if you want to ignore certain namespaces from the result.
        /// </summary>
        public static readonly HashSet<string> HiddenNamespaces = new()
        {
            "JetBrains.Profiler.Core.Instrumentation"
        };

    }
}