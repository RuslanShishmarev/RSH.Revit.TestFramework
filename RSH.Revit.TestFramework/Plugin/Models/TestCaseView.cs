using System;
using System.Collections.Generic;
using System.Reflection;

namespace RSH.Revit.TestFramework.Models
{
    internal class TestCaseView
    {
        public string Name => Method?.Name;

        public string ParentName => Method?.DeclaringType.Name;

        public string ResultText { get; private set; }

        public bool? Result { get; private set; }

        public string StackTrace { get; private set; }

        public object Argument1 { get; private set; }

        public object Argument2 { get; private set; }

        public MethodInfo Method { get; private set; }

        private object _testClassInstance;

        public TestCaseView(object testClassInstance, object argument, MethodInfo method)
        {
            _testClassInstance = testClassInstance;
            Argument1 = argument;
            Method = method;
            Result = null;
        }

        public TestCaseView(object testClassInstance, object argument1, object argument2, MethodInfo method)
        {
            _testClassInstance = testClassInstance;
            Argument1 = argument1;
            Argument2 = argument2;
            Method = method;
            Result = null;
        }

        public void Run()
        {
            try
            {
                var argumentsToInvoke = new List<object>();
                if (Argument1 != null)
                {
                    argumentsToInvoke.Add(Argument1);
                }
                if (Argument2 != null)
                {
                    argumentsToInvoke.Add(Argument2);
                }

                Method.Invoke(_testClassInstance, argumentsToInvoke.ToArray());
                ResultText = "Succeded";
                Result = true;
            }
            catch (Exception ex)
            {
                ResultText = ex.InnerException?.Message;
                Result = false;
                StackTrace = ex.InnerException?.StackTrace;
            }
        }

        public override string ToString()
        {
            string viewName = string.Empty;
            if (Argument1 != null)
            {
                viewName += Argument1.ToString();
            }
            if (Argument2 != null)
            {
                viewName += $" && {Argument2}";
            }

            return viewName;
        }
    }
}
