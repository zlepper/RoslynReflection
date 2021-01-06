using System;
using System.Linq;
using NUnit.Framework;
using RoslynReflection.Extensions;

namespace RoslynReflection.Test.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void SkipLast_ThrowsOutOfRangeIfCountLessThanZero()
        {
            Assert.That(() =>
            {
                EnumerableExtensions.SkipLast(Enumerable.Range(0, 4), -1);
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}