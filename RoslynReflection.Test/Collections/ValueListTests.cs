using NUnit.Framework;
using RoslynReflection.Collections;

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
            
            Assert.That(list1, Is.EqualTo(list2));
        }

        [Test]
        public void ComparesContent_NotEquals()
        {
            var list1 = new ValueList<int> {1, 2, 4};
            var list2 = new ValueList<int> {1, 2, 3};
            
            Assert.That(list1, Is.Not.EqualTo(list2));
        }
    }
}