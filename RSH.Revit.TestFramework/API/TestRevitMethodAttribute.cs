using System;

namespace RSH.Revit.TestFramework.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class TestRevitMethodAttribute : Attribute
    {
        public object Argument1 { get; private set; }
        public object Argument2 { get; private set; }

        public TestRevitMethodAttribute(object argument)
        {
            Argument1 = argument;
        }

        public TestRevitMethodAttribute(object argument1, object argument2)
        {
            Argument1 = argument1;
            Argument2 = argument2;
        }

        public TestRevitMethodAttribute() { }
    }
}
