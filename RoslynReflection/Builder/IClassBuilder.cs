using System;

namespace RoslynReflection.Builder
{
    public interface IClassBuilder : INamespaceBuilder
    {
        /// <summary>
        /// Creates a new inner class for this class
        /// </summary>
        /// <param name="name">The name of the inner class</param>
        /// <returns>A builder for modifying the inner class</returns>
        IClassBuilder NewInnerClass(string name);
        
        /// <summary>
        /// Navigates back to the outer class
        /// </summary>
        /// <returns>The builder for the outer class</returns>
        IClassBuilder GoBackToParent();

        /// <summary>
        /// Adds a new attribute instance to this class
        /// </summary>
        /// <param name="attribute">The attribute instance to pass along</param>
        /// <returns>This builder</returns>
        IClassBuilder WithAttribute(Attribute attribute);
    }
}