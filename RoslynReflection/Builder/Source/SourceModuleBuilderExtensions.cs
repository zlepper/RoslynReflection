﻿using RoslynReflection.Models;

namespace RoslynReflection.Builder.Source
{
    public static class SourceModuleBuilderExtensions
    {
        public static ScannedNamespace AddNamespace(this ScannedModule module, string name)
        {
            return new(module, name);
        }
    }
}