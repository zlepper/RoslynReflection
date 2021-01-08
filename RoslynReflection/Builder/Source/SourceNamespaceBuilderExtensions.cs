﻿using RoslynReflection.Models;
using RoslynReflection.Models.Source;

namespace RoslynReflection.Builder.Source
{
    public static class SourceNamespaceBuilderExtensions
    {
        public static ScannedSourceClass AddSourceClass(this ScannedNamespace ns, string name)
        {
            return new(ns, name);
        }

        public static ScannedSourceInterface AddSourceInterface(this ScannedNamespace ns, string name)
        {
            return new(ns, name);
        }
    }
}