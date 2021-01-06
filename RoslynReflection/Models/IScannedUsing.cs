﻿using JetBrains.Annotations;

namespace RoslynReflection.Models
{
    public interface IScannedUsing
    {
        [ContractAnnotation("=> true, type: notnull; => false, type: null")]
        internal bool TryGetType(string typeName, AvailableTypes availableTypes,
            out ScannedType? type);
    }
}