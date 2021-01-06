using NUnit.Framework;
using RoslynReflection.Models;
using RoslynReflection.Models.Source;

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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass");

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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
            var otherClass = new ScannedSourceClass(classNamespace, "OtherClass");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass");

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
            var myClass = new ScannedSourceClass(classNamespace, "MyClass");
            var inner = new ScannedSourceClass(classNamespace, "Inner", myClass);
            

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
            var myClass = new ScannedSourceClass(classNamespace, "MyClass");
            var inner = new ScannedSourceClass(classNamespace, "Inner", myClass);
            var innerInner = new ScannedSourceClass(classNamespace, "InnerInner", inner);
            

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
            var otherClass = new ScannedSourceClass(classNamespace, "OtherClass");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass");
            var inner = new ScannedSourceClass(classNamespace, "Inner", otherClass);

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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var inner = new ScannedSourceClass(otherNamespace, "Inner", otherClass);
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsing(otherNamespace.Name)
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(classNamespace);
            available.AddNamespace(otherNamespace);
            
            Assert.That(available.TryGetType(myClass, "OtherClass.Inner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, inner), Is.True);
        }

        [Test]
        public void FindsTypesOnAliasImports()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
            var unused = new ScannedSourceClass(otherNamespace, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
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
        
        [Test]
        public void SkipsConflictingNamespaceIfAliasDoesntMatch()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var otherNamespace2 = new ScannedNamespace(module, "OtherNamespace2");
            var otherClass2 = new ScannedSourceClass(otherNamespace2, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace", "MyThing"),
                    new ScannedUsingAlias("OtherNamespace2", "MyThing2")
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(otherNamespace2);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"MyThing2.{otherClass2.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass2), Is.True);
        }
        
        [Test]
        public void FindsClassInNormalNamespaceEvenWithAliasImports()
        {
            var module = new ScannedModule();
            var otherNamespace = new ScannedNamespace(module, "OtherNamespace");
            var otherClass = new ScannedSourceClass(otherNamespace, "OtherClass");
            var otherNamespace2 = new ScannedNamespace(module, "OtherNamespace2");
            var otherClass2 = new ScannedSourceClass(otherNamespace2, "OtherClass");
            var classNamespace = new ScannedNamespace(module, "MyNamespace");
            var myClass = new ScannedSourceClass(classNamespace, "MyClass")
            {
                Usings =
                {
                    new ScannedUsingAlias("OtherNamespace", "MyThing"),
                    new ScannedUsing("OtherNamespace2"),
                }
            };

            var available = new AvailableTypes();
            available.AddNamespace(otherNamespace);
            available.AddNamespace(otherNamespace2);
            available.AddNamespace(classNamespace);
            
            Assert.That(available.TryGetType(myClass, $"{otherClass2.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match, otherClass2), Is.True);
        }

    }
}