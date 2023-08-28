using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.API.Interfaces;

namespace RSH.Revit.TestFramework.API
{
    internal abstract class TestRevitBase
    {
        private IConsoleTestBrowser _console;
        protected ExternalCommandData CommandData;
        protected Document Doc;
        public void Setup(IConsoleTestBrowser console, ExternalCommandData commandData)
        {
            _console = console;
            CommandData = commandData;
            Doc = commandData.Application.ActiveUIDocument.Document;
        }

        protected void WriteLine(string text)
        {
            _console.SetConsoleText(text);
        }
    }
}