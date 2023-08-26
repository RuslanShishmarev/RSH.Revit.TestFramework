using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.Models;

using System.Collections.Generic;

namespace RSH.Revit.TestFramework.Commands
{
    internal class RunTestsHandler : IExternalEventHandler
    {
        private IEnumerable<TestCaseView> _testCaseViews;

        private ExternalEvent _externalEvent;
        public void Initialize()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Execute(UIApplication app)
        {
            foreach (var test in _testCaseViews)
            {
                test.Run(app.ActiveUIDocument.Document);
            }           
        }

        public void Raise(IEnumerable<TestCaseView> testCaseViews)
        {
            _testCaseViews = testCaseViews;
            _externalEvent.Raise();
        }

        public string GetName()
        {
            return "Run tests";
        }
    }
}
