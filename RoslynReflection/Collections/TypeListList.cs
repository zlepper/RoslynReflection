using RoslynReflection.Models;

namespace RoslynReflection.Collections
{
    internal class TypeListList
    {
        internal readonly ClassList ClassList;
        internal readonly InterfaceList InterfaceList;
        internal readonly RecordList RecordList;

        internal TypeListList(ScannedNamespace ns)
        {
            ClassList = new ClassList(ns);
            InterfaceList = new InterfaceList(ns);
            RecordList = new RecordList(ns);
        }
    }
}