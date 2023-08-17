using System;

namespace RSH.Revit.TestFramework.Api
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class TestRevitClassAttribute : Attribute
    {
        public string Name { get; private set; }

        public TestRevitClassAttribute(string name)
        {
            Name = name;
        }

        public TestRevitClassAttribute()
        {
        }
    }
}
