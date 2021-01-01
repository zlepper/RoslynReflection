using System.Text;
using NUnit.Framework;
using RoslynReflection.Extensions;

namespace RoslynReflection.Test.Extensions
{
    [TestFixture]
    public class StringBuilderExtensionsTests
    {
        [Test]
        public void AppendsSingleField()
        {
            var builder = new StringBuilder();
            builder.AppendField("MyField", "MyValue");

            var result = builder.ToString();
            
            Assert.That(result, Is.EqualTo("MyField = MyValue"));
        }

        [Test]
        public void AppendsMultipleFields()
        {
            var builder = new StringBuilder();
            builder.AppendField("MyFirstField", "MyFirstValue")
                .AppendField("MySecondField", "MySecondValue");
            
            var result = builder.ToString();
            
            Assert.That(result, Is.EqualTo("MyFirstField = MyFirstValue, MySecondField = MySecondValue"));
        }
    }
}