using System;
using NUnit.Framework;
using RoslynReflection.Collections;
using RoslynReflection.Helpers;
using RoslynReflection.Models;

namespace RoslynReflection.Test.Collections
{
    
    [TestFixture]
    public class ValueListTests
    {
        [Test]
        public void ComparesContent_Equals()
        {
            var list1 = new ValueList<int> {1, 2, 3};
            var list2 = new ValueList<int> {1, 2, 3};
            
            Assert.That(list1.Equals(list2), Is.True);
        }

        [Test]
        public void ComparesContent_NotEquals()
        {
            var list1 = new ValueList<int> {1, 2, 4};
            var list2 = new ValueList<int> {1, 2, 3};
            
            Assert.That(list1.Equals(list2), Is.False);
        }

        [Test]
        public void ComparesIgnoringOrder()
        {
            var list1 = new ValueList<int> {1, 2, 3};
            var list2 = new ValueList<int> {3, 2, 1};
            
            Assert.That(list1.Equals(list2), Is.True);
        }

        [Test]
        public void ComparesAttributeUsageAttributes()
        {
            var list1 = new ValueList<object>(AttributeComparer.Instance)
                {new AttributeUsageAttribute(AttributeTargets.Class)};
            var list2 = new ValueList<object>(AttributeComparer.Instance)
                {new AttributeUsageAttribute(AttributeTargets.Class)};
            
            Assert.That(list1.Equals(list2), Is.True);
        }
    }
}