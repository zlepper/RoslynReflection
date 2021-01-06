namespace RoslynReflection.Builder
{
    public interface IClassBuilder : INamespaceBuilder
    {
        /// <summary>
        /// Creates a new inner class for this class
        /// </summary>
        /// <param name="name">The name of the inner class</param>
        /// <returns>A builder for modifying the inner class</returns>
        INestedClassBuilder<IClassBuilder> NewInnerClass(string name);
        
        /// <summary>
        /// Adds a new attribute instance to this class
        /// </summary>
        /// <param name="attribute">The attribute instance to pass along</param>
        /// <returns>This builder</returns>
        IClassBuilder WithAttribute(object attribute);
    }

    public interface INestedClassBuilder<out TClassBuilder> : IClassBuilder
        where TClassBuilder : IClassBuilder
    {
        /// <summary>
        /// Navigates back to the outer class
        /// </summary>
        /// <returns>The builder for the outer class</returns>
        TClassBuilder GoBackToParent();

        /// <inheritdoc cref="IClassBuilder.NewInnerClass" />
        new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(string name);
    }
}