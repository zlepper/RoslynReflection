﻿using RoslynReflection.Collections;

namespace RoslynReflection.Models
{
    public record CompilationAnalysisResult
    {
        public readonly ScannedModule MainModule;
        public readonly ValueDictionary<ScannedModule, ValueList<ScannedModule>> Dependencies = new();

        public CompilationAnalysisResult(ScannedModule mainModule)
        {
            MainModule = mainModule;
        }
    }
}