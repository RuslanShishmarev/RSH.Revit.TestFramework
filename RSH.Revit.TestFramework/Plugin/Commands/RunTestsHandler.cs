using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Collections.Generic;

using RSH.Revit.TestFramework.Models;

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
            using (Transaction testTr = new Transaction(app.ActiveUIDocument.Document))
            {
                testTr.Start("Tests");
                foreach (var test in _testCaseViews)
                {
                    test.Run();
                }
                testTr.Commit();
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
