﻿using NUnit.Framework;
using RoslynReflection.Builder.Source;
using RoslynReflection.Models;
using RoslynReflection.Test.TestHelpers.TestAttributes;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class ScannedClassTests
    {
        [Test]
        public void ComparesAttributes()
        {
            var left = new ScannedModule()
                .AddNamespace("ns")
                .AddSourceClass("MyClass")
                .AddAttribute(new SampleAttribute("hello"))
                .Module;

            var right = new ScannedModule()
                .AddNamespace("ns")
                .AddSourceClass("MyClass")
                .AddAttribute(new SampleAttribute("world"))
                .Module;

            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void ToStringReturnsSomething()
        {
            var klass = new ScannedModule()
                .AddNamespace("ns")
                .AddSourceClass("MyClass");

            Assert.That(klass.ToString(), Is.Not.Empty);
        }

        [Test]
        public void Equals_ReturnsTrueIfReferenceAreTheSame()
        {
            var klass = new ScannedModule()
                .AddNamespace("ns")
                .AddSourceClass("MyClass");

            Assert.That(klass.Equals(klass));
        }
    }
}