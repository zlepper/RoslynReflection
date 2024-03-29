﻿using System.Linq;
using NUnit.Framework;
using RoslynReflection.Builder;
using RoslynReflection.Models;
using RoslynReflection.Models.Extensions;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        private ScannedModule GetTestModule()
        {
            return new ScannedModule()
                .AddNamespace("MyNamespace")
                .AddClass("MyClass")
                .AddNestedClass("MyInnerClass")
                .Module;
        }
        
        [Test]
        public void IsNestedType_Yes()
        {
            var nested = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyInnerClass");

            Assert.That(nested.IsNestedType(), Is.True); 
        }

        [Test]
        public void IsNestedType_No()
        {
            var nested = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyClass");

            Assert.That(nested.IsNestedType(), Is.False); 
        }

        [Test]
        public void FullName_ForRootClass()
        {
            var target = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyClass");
            
            Assert.That(target.FullName(), Is.EqualTo("MyClass"));
        }
        
        [Test]
        public void FullName_ForNestedClass()
        {
            var target = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyInnerClass");
            
            Assert.That(target.FullName(), Is.EqualTo("MyClass.MyInnerClass"));
        }
        
        [Test]
        public void FullyQualifiedName_ForRootClass()
        {
            var target = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyClass");
            
            Assert.That(target.FullyQualifiedName(), Is.EqualTo("MyNamespace.MyClass"));
        }
        
        [Test]
        public void FullyQualifiedName_ForNestedClass()
        {
            var target = GetTestModule()
                .Classes()
                .Single(c => c.Name == "MyInnerClass");
            
            Assert.That(target.FullyQualifiedName(), Is.EqualTo("MyNamespace.MyClass.MyInnerClass"));
        }

        [Test]
        public void FullyQualifiedName_ForClassWithoutNamespace()
        {
            var target = new ScannedModule()
                .AddNamespace("")
                .AddClass("MyClass");
            
            Assert.That(target.FullyQualifiedName(), Is.EqualTo("MyClass"));
        }
    }
}