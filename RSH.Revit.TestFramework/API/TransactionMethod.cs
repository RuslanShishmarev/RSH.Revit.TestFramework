using System;

namespace RSH.Revit.TestFramework.API
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class TransactionMethod : Attribute
    {
        public string Name { get; }

        public TransactionMethod(string name)
        {
            Name = name;
        }

        public TransactionMethod() { }
    }
}
