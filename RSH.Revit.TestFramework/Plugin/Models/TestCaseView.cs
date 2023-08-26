using Autodesk.Revit.DB;

using RSH.Revit.TestFramework.API;

using System;
using System.Collections.Generic;
using System.Linq;
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

        public object Argument1 { get; }

        public object Argument2 { get; }

        public MethodInfo Method { get; }

        public bool WithTransaction { get; }

        private string _transactionName;

        private object _testClassInstance;

        public TestCaseView(
            object testClassInstance, 
            object argument, 
            MethodInfo method,
            bool withTransaction,
            string transactionName)
        {
            _testClassInstance = testClassInstance;
            Argument1 = argument;
            Method = method;
            WithTransaction = withTransaction;
            _transactionName = transactionName ?? Method?.Name;
            Result = null;
        }

        public TestCaseView(
            object testClassInstance, 
            object argument1, 
            object argument2, 
            MethodInfo method,
            bool withTransaction,
            string transactionName)
        {
            _testClassInstance = testClassInstance;
            Argument1 = argument1;
            Argument2 = argument2;
            Method = method;
            WithTransaction = withTransaction;
            _transactionName = transactionName ?? Method?.Name;
            Result = null;
        }

        public void Run(Document doc)
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

                var methodDelegate = new Action(() =>
                {
                    Method.Invoke(_testClassInstance, argumentsToInvoke.ToArray());
                });

                if (WithTransaction)
                {
                    using (Transaction testTr = new Transaction(doc))
                    {
                        testTr.Start(_transactionName);

                        methodDelegate.Invoke();

                        testTr.Commit();
                    }
                }
                else
                {
                    methodDelegate.Invoke();
                }

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
                viewName += $", {Argument2}";
            }

            return viewName;
        }
    }
}
