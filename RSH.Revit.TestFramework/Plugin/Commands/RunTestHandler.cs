using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.Models;

namespace RSH.Revit.TestFramework.Commands
{
    internal class RunTestHandler : IExternalEventHandler
    {
        private TestCaseView _testCaseView;

        private ExternalEvent _externalEvent;
        public void Initialize()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Execute(UIApplication app)
        {
            using (Transaction testTr = new Transaction(app.ActiveUIDocument.Document))
            {
                testTr.Start(_testCaseView.Name);

                _testCaseView.Run();

                testTr.Commit();
            }
        }

        public void Raise(TestCaseView testCaseView)
        {
            _testCaseView = testCaseView;
            _externalEvent.Raise();
        }

        public string GetName()
        {
            return "Run test";
        }
    }
}
