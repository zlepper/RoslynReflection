namespace RoslynReflection.Builder.Source
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

        /// <summary>
        /// Adds the specified namespace as an import
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IClassBuilder WithUsing(string ns);

        /// <summary>
        /// Adds the specified namespace as an alias import
        /// </summary>
        /// <param name="ns">Namespace</param>
        /// <param name="alias">The alias the namespace should have</param>
        /// <returns></returns>
        IClassBuilder WithAliasUsing(string ns, string alias);
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

        /// <inheritdoc cref="IClassBuilder.WithAttribute" />
        new INestedClassBuilder<TClassBuilder> WithAttribute(object attribute);

        /// <inheritdoc cref="IClassBuilder.WithUsing" />
        new INestedClassBuilder<TClassBuilder> WithUsing(string ns);
        
        /// <inheritdoc cref="IClassBuilder.WithAliasUsing" />
        new INestedClassBuilder<TClassBuilder> WithAliasUsing(string ns, string alias);
    }
}