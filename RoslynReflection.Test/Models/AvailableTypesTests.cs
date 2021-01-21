using NUnit.Framework;
using RoslynReflection.Helpers;
using RoslynReflection.Parsers;
using RoslynReflection.Parsers.SourceCode.Models;
using RoslynReflection.Test.TestHelpers.Extensions;

namespace RoslynReflection.Test.Models
{
    [TestFixture]
    public class AvailableTypesTests
    {
        [Test]
        public void FindsClassInOtherNamespace_ThroughImport()
        {
            var module = new RawScannedModule();
            var otherClass = module.AddNamespace("OtherNamespace")
                .AddType("OtherClass");
            var myClass = module.AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsing("OtherNamespace");


            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void DoesNotFindClassWithoutImportEvenIfClassExist()
        {
            var module = new RawScannedModule();
            var otherClass = module.AddNamespace("OtherNamespace")
                .AddType("OtherClass");
            var myClass = module.AddNamespace("MyNamespace")
                .AddType("MyClass");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.False);
            Assert.That(match, Is.Null);
        }

        [Test]
        public void FindsClassInOtherNamespace_UsingFullyQualifiedTypeName()
        {
            var module = new RawScannedModule();
            var otherClass = module.AddNamespace("OtherNamespace")
                .AddType("OtherClass");
            var myClass = module.AddNamespace("MyNamespace")
                .AddType("MyClass");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, otherClass.FullyQualifiedName(), out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void FindsClassInSameNamespaceWithoutImport()
        {
            var module = new RawScannedModule();
            var classNamespace = module.AddNamespace("MyNamespace");
            var otherClass = classNamespace.AddType("OtherClass");
            var myClass = classNamespace.AddType("MyClass");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, otherClass.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void FindsInnerClassesWithoutQualification()
        {
            var module = new RawScannedModule();
            var classNamespace = module.AddNamespace("MyNamespace");
            var myClass = classNamespace.AddType("MyClass");
            var inner = myClass.AddNestedType("Inner");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, inner.Name, out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, inner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InSameClass()
        {
            var module = new RawScannedModule();
            var classNamespace = module.AddNamespace("MyNamespace");
            var myClass = classNamespace.AddType("MyClass");
            var inner = myClass.AddNestedType("Inner");
            var innerInner = inner.AddNestedType("InnerInner");


            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, "Inner.InnerInner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, innerInner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InAnotherClass_SameNamespace()
        {
            var module = new RawScannedModule();
            var classNamespace = module.AddNamespace("MyNamespace");
            var otherClass = classNamespace.AddType("OtherClass");
            var inner = otherClass.AddNestedType("Inner");
            var myClass = classNamespace.AddType("MyClass");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, "OtherClass.Inner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, inner), Is.True);
        }

        [Test]
        public void FindsNestedInnerClasses_InAnotherClass_DifferentNamespace()
        {
            var module = new RawScannedModule();
            var classNamespace = module.AddNamespace("MyNamespace");
            var otherNamespace = module.AddNamespace("OtherNamespace");
            var otherClass = otherNamespace.AddType("OtherClass");
            var inner = otherClass.AddNestedType("Inner");
            var myClass = classNamespace.AddType("MyClass")
                .AddUsing(otherNamespace.Name);

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, "OtherClass.Inner", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, inner), Is.True);
        }

        [Test]
        public void FindsTypesOnAliasImports()
        {
            var module = new RawScannedModule();
            var otherNamespace = module.AddNamespace("OtherNamespace");
            var otherClass = otherNamespace.AddType("OtherClass");
            var classNamespace = module.AddNamespace("MyNamespace");
            var myClass = classNamespace.AddType("MyClass")
                .AddUsingAlias("OtherNamespace", "MyThing");


            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, $"MyThing.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void FindsTypesOnAliasImports_InNestedNamespace()
        {
            var module = new RawScannedModule();
            var otherNamespace = module.AddNamespace("OtherNamespace.Inner");
            var otherClass = otherNamespace.AddType("OtherClass");
            var classNamespace = module.AddNamespace("MyNamespace");
            var myClass = classNamespace.AddType("MyClass")
                .AddUsingAlias("OtherNamespace", "MyThing");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, $"MyThing.Inner.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void FindsTypesOnAliasImports_InNestedNamespace2()
        {
            var module = new RawScannedModule();
            var otherNamespace = module.AddNamespace("OtherNamespace.Inner");
            var otherClass = otherNamespace.AddType("OtherClass");
            var classNamespace = module.AddNamespace("MyNamespace");
            var myClass = classNamespace.AddType("MyClass")
                .AddUsingAlias("OtherNamespace.Inner", "MyThing");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, $"MyThing.{otherClass.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass), Is.True);
        }

        [Test]
        public void ReturnsFalseIfNotFoundInAlias()
        {
            var myClass = new RawScannedModule()
                .AddNamespace("OtherNamespace")
                .AddType("OtherClass")
                .Module
                .AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsingAlias("OtherNamespace", "MyThing");
            var module = myClass.Module;

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, "MyThing.NoExist", out var match), Is.False);
            Assert.That(match, Is.Null);
        }

        [Test]
        public void SkipsConflictingNamespaceIfAliasDoesntMatch()
        {
            var module = new RawScannedModule();
            var otherClass2 = module.AddNamespace("OtherNamespace")
                .AddType("OtherClass")
                .Module
                .AddNamespace("OtherNamespace2")
                .AddType("OtherClass");
            var myClass = module.AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsingAlias("OtherNamespace", "MyThing")
                .AddUsingAlias("OtherNamespace2", "MyThing2");


            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);


            Assert.That(available.TryGetType(myClass, $"MyThing2.{otherClass2.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass2), Is.True);
        }


        [Test]
        public void FindsClassInNormalNamespaceEvenWithAliasImports()
        {
            var module = new RawScannedModule();
            var otherClass2 = module.AddNamespace("OtherNamespace")
                .AddType("OtherClass")
                .Module
                .AddNamespace("OtherNamespace2")
                .AddType("OtherClass");

            var myClass = module.AddNamespace("MyNamespace")
                .AddType("MyClass")
                .AddUsingAlias("OtherNamespace", "MyThing2")
                .AddUsing("OtherNamespace2");

            var scannedModule = RawScannedTypeLinker.GetUnlinkedScannedModule(module);
            var available = new AvailableTypes(scannedModule);

            Assert.That(available.TryGetType(myClass, $"{otherClass2.Name}", out var match), Is.True);
            Assert.That(match, Is.Not.Null);
            Assert.That(ReferenceEquals(match!.RawScannedType, otherClass2), Is.True);
        }
    }
}