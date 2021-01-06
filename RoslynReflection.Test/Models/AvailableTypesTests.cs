using NUnit.Framework;
using RoslynReflection.Models;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class AvailableTypesTests
    {
        [Test]
        public void FindsClassInOtherNamespace_ThroughImport()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsing("OtherNamespace")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }
        
        [Test]
        public void DoesNotFindClassWithoutImportEvenIfClassExist()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass");

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.False);
            Assert.That(match, Is.Null);
        }
        
        [Test]
        public void FindsClassInOtherNamespace_UsingFullyQualifiedTypeName()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsing("OtherNamespace")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, otherClass.FullyQualifiedName(), out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }
        
        [Test]
        public void FindsClassInSameNamespaceWithoutImport()
        {
            var module = new ScannedModule();
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var otherClass = new ScannedClass(classNamespace, "OtherClass");
            var myClass = new ScannedClass(classNamespace, "MyClass");

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }

        [Test]
        public void FindsInnerClassesWithoutQualification()
        {
            var module = new ScannedModule();
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass");
            var inner = new ScannedClass(classNamespace, "Inner", myClass);
            

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, inner.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, inner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InSameClass()
        {
            var module = new ScannedModule();
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass");
            var inner = new ScannedClass(classNamespace, "Inner", myClass);
            var innerInner = new ScannedClass(classNamespace, "InnerInner", inner);
            

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, "Inner.InnerInner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, innerInner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InAnotherClass_SameNamespace()
        {
            var module = new ScannedModule();
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var otherClass = new ScannedClass(classNamespace, "OtherClass");
            var myClass = new ScannedClass(classNamespace, "MyClass");
            var inner = new ScannedClass(classNamespace, "Inner", otherClass);

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, "OtherClass.Inner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, inner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InAnotherClass_DifferentNamespace()
        {
            var module = new ScannedModule();
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var inner = new ScannedClass(otherNamespace, "Inner", otherClass);
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsing(otherNamespace.Name)
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, "OtherClass.Inner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, inner), Is.True);
        }

        [Test]
        public void FindsTypesOnAliasImports()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace", "MyThing")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"MyThing.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }
        
        [Test]
        public void FindsTypesOnAliasImports_InNestedNamespace()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace.Inner");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace", "MyThing")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"MyThing.Inner.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }
        
        [Test]
        public void FindsTypesOnAliasImports_InNestedNamespace2()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace.Inner");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace.Inner", "MyThing")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"MyThing.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass), Is.True);
        }
        
        [Test]
        public void ReturnsFalseIfNotFoundInAlias()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace", "MyThing")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"MyThing.NoExist", out var match), Is.False);
            Assert.That(match, Is.Null);
        }

    }
}