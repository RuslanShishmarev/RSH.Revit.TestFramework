using System;

namespace RSH.Revit.TestFramework.API
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class TransactionMethodAttribute : Attribute
    {
        public string Name { get; }

        public TransactionMethodAttribute(string name)
        {
            Name = name;
        }

        public TransactionMethodAttribute() { }
    }
}
