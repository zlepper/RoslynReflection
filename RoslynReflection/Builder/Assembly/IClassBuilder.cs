using System;

namespace RoslynReflection.Builder.Assembly
{
    public interface IClassBuilder : INamespaceBuilder
    {
        /// <summary>
        /// Creates a new inner class for this class
        /// </summary>
        /// <param name="type">The type of the class</param>
        /// <returns>A builder for modifying the inner class</returns>
        INestedClassBuilder<IClassBuilder> NewInnerClass(Type type);

        INestedClassBuilder<IClassBuilder> NewInnerClass<T>();

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
        new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass(Type type);

        new INestedClassBuilder<INestedClassBuilder<TClassBuilder>> NewInnerClass<T>();
    }
}